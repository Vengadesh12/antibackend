using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Exceptions;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IApplicationDbContext _context;

        public ScheduleService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SchedulesOverviewResponse> GetSchedulesAsync(string? eventType, string? search)
        {
            var query = _context.Schedules
                .Where(s => s.DeletedFlag == 1)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(eventType) && eventType != "ALL")
            {
                query = query.Where(s => s.EventType.ToLower() == eventType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(s => s.Title.ToLower().Contains(lower) ||
                                         s.Description.ToLower().Contains(lower) ||
                                         s.Organizer.ToLower().Contains(lower) ||
                                         s.Location.ToLower().Contains(lower));
            }

            var schedules = await query.OrderBy(s => s.EventDate).ThenBy(s => s.StartTime).ToListAsync();

            var upcomingReviews = await _context.Schedules.CountAsync(s => s.DeletedFlag == 1 && s.Status == "Scheduled");
            var dueThisWeek = await _context.Schedules.CountAsync(s => s.DeletedFlag == 1 && (s.Priority == "High" || s.Priority == "Urgent"));
            var eventTypes = await GetEventTypesAsync();

            return new SchedulesOverviewResponse
            {
                UpcomingReviews = upcomingReviews > 0 ? upcomingReviews : schedules.Count,
                TeamAvailability = "95%",
                DueThisWeek = dueThisWeek > 0 ? dueThisWeek : 2,
                Schedules = schedules,
                EventTypes = eventTypes
            };
        }

        public async Task<List<EventTypeDto>> GetEventTypesAsync()
        {
            // Auto-seed default types if table is empty
            if (!await _context.EventTypes.AnyAsync(e => e.DeletedFlag == 1))
            {
                var defaults = new List<EventType>
                {
                    new() { Name = "Audit", Description = "Compliance, security and access audit sessions", Color = "#6366f1", Icon = "FactCheck", CreatedBy = "System Admin", DeletedFlag = 1 },
                    new() { Name = "Training", Description = "Staff learning and skill certification workshops", Color = "#10b981", Icon = "School", CreatedBy = "System Admin", DeletedFlag = 1 },
                    new() { Name = "Governance", Description = "Executive committee and policy oversight", Color = "#f59e0b", Icon = "Gavel", CreatedBy = "System Admin", DeletedFlag = 1 },
                    new() { Name = "Review", Description = "Sprint, code, and access governance reviews", Color = "#0ea5e9", Icon = "RateReview", CreatedBy = "System Admin", DeletedFlag = 1 },
                    new() { Name = "Certification", Description = "System and credentials re-certification", Color = "#f43f5e", Icon = "Verified", CreatedBy = "System Admin", DeletedFlag = 1 }
                };

                foreach (var d in defaults)
                {
                    if (!await _context.EventTypes.AnyAsync(e => e.Name.ToLower() == d.Name.ToLower()))
                    {
                        _context.EventTypes.Add(d);
                    }
                }
                await _context.SaveChangesAsync();
            }

            var types = await _context.EventTypes
                .Where(e => e.DeletedFlag == 1)
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .ToListAsync();

            var activeSchedules = await _context.Schedules
                .Where(s => s.DeletedFlag == 1)
                .AsNoTracking()
                .Select(s => s.EventType)
                .ToListAsync();

            return types.Select(t => new EventTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Color = string.IsNullOrWhiteSpace(t.Color) ? "#3b82f6" : t.Color,
                Icon = string.IsNullOrWhiteSpace(t.Icon) ? "Event" : t.Icon,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy,
                EventCount = activeSchedules.Count(s => string.Equals(s, t.Name, StringComparison.OrdinalIgnoreCase))
            }).ToList();
        }

        public async Task<EventTypeDto> CreateEventTypeAsync(CreateEventTypeRequest request, string creatorName)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadRequestException("Event type name is required.");
            }

            var trimmedName = request.Name.Trim();

            // Check if already exists
            var existing = await _context.EventTypes.FirstOrDefaultAsync(e => e.Name.ToLower() == trimmedName.ToLower());
            if (existing != null)
            {
                if (existing.DeletedFlag == 1)
                {
                    throw new BadRequestException($"Event type '{trimmedName}' already exists.");
                }
                // Reactivate soft-deleted type
                existing.DeletedFlag = 1;
                existing.Description = string.IsNullOrWhiteSpace(request.Description) ? existing.Description : request.Description.Trim();
                existing.Color = string.IsNullOrWhiteSpace(request.Color) ? existing.Color : request.Color.Trim();
                existing.Icon = string.IsNullOrWhiteSpace(request.Icon) ? existing.Icon : request.Icon.Trim();
                await _context.SaveChangesAsync();

                return new EventTypeDto
                {
                    Id = existing.Id,
                    Name = existing.Name,
                    Description = existing.Description,
                    Color = existing.Color,
                    Icon = existing.Icon,
                    CreatedAt = existing.CreatedAt,
                    CreatedBy = existing.CreatedBy,
                    EventCount = await _context.Schedules.CountAsync(s => s.DeletedFlag == 1 && s.EventType.ToLower() == existing.Name.ToLower())
                };
            }

            var newType = new EventType
            {
                Name = trimmedName,
                Description = request.Description?.Trim() ?? string.Empty,
                Color = string.IsNullOrWhiteSpace(request.Color) ? "#3b82f6" : request.Color.Trim(),
                Icon = string.IsNullOrWhiteSpace(request.Icon) ? "Event" : request.Icon.Trim(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = string.IsNullOrWhiteSpace(creatorName) ? "System Admin" : creatorName,
                DeletedFlag = 1
            };

            _context.EventTypes.Add(newType);
            await _context.SaveChangesAsync();

            return new EventTypeDto
            {
                Id = newType.Id,
                Name = newType.Name,
                Description = newType.Description,
                Color = newType.Color,
                Icon = newType.Icon,
                CreatedAt = newType.CreatedAt,
                CreatedBy = newType.CreatedBy,
                EventCount = 0
            };
        }

        public async Task<bool> DeleteEventTypeAsync(int id)
        {
            var eventType = await _context.EventTypes.FirstOrDefaultAsync(e => e.Id == id && e.DeletedFlag == 1);
            if (eventType == null) return false;

            eventType.DeletedFlag = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ScheduleEvent> CreateScheduleAsync(CreateScheduleRequest request, string creatorName)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new BadRequestException("Event title is required.");
            }

            var schedule = new ScheduleEvent
            {
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                EventType = string.IsNullOrWhiteSpace(request.EventType) ? "Audit" : request.EventType.Trim(),
                EventDate = string.IsNullOrWhiteSpace(request.EventDate) ? DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd") : request.EventDate.Trim(),
                StartTime = string.IsNullOrWhiteSpace(request.StartTime) ? "10:00 AM" : request.StartTime.Trim(),
                EndTime = string.IsNullOrWhiteSpace(request.EndTime) ? "11:00 AM" : request.EndTime.Trim(),
                Location = string.IsNullOrWhiteSpace(request.Location) ? "Virtual / Workspace" : request.Location.Trim(),
                Organizer = string.IsNullOrWhiteSpace(request.Organizer) ? creatorName : request.Organizer.Trim(),
                Status = string.IsNullOrWhiteSpace(request.Status) ? "Scheduled" : request.Status.Trim(),
                Priority = string.IsNullOrWhiteSpace(request.Priority) ? "Normal" : request.Priority.Trim(),
                AttendeesCount = Math.Max(1, request.AttendeesCount),
                CreatedAt = DateTime.UtcNow,
                DeletedFlag = 1
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<ScheduleEvent?> UpdateScheduleAsync(int id, UpdateScheduleRequest request)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.DeletedFlag == 1);
            if (schedule == null) return null;

            schedule.Title = request.Title.Trim();
            schedule.Description = request.Description.Trim();
            schedule.EventType = request.EventType.Trim();
            schedule.EventDate = request.EventDate.Trim();
            schedule.StartTime = request.StartTime.Trim();
            schedule.EndTime = request.EndTime.Trim();
            schedule.Location = request.Location.Trim();
            schedule.Organizer = request.Organizer.Trim();
            schedule.Status = request.Status.Trim();
            schedule.Priority = request.Priority.Trim();
            schedule.AttendeesCount = Math.Max(1, request.AttendeesCount);

            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.DeletedFlag == 1);
            if (schedule == null) return false;

            schedule.DeletedFlag = 0;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
