using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Exceptions;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationDbContext _context;

        public ProjectService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectsOverviewResponse> GetProjectsAsync(string? category, string? status, string? search)
        {
            var query = _context.Projects
                .Where(p => p.DeletedFlag == 1)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category) && category != "ALL")
            {
                query = query.Where(p => p.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "ALL")
            {
                query = query.Where(p => p.Status.ToLower() == status.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lower) ||
                                         p.Description.ToLower().Contains(lower) ||
                                         p.LeadName.ToLower().Contains(lower));
            }

            var projects = await query.OrderByDescending(p => p.Id).ToListAsync();

            var activeRollouts = await _context.Projects.CountAsync(p => p.DeletedFlag == 1 && p.Status == "In Progress");
            var onTrackCount = await _context.Projects.CountAsync(p => p.DeletedFlag == 1 && p.ProgressPercentage >= 50);
            var pendingReviews = await _context.Projects.CountAsync(p => p.DeletedFlag == 1 && p.Status == "Review");

            return new ProjectsOverviewResponse
            {
                ActiveRollouts = activeRollouts > 0 ? activeRollouts : projects.Count,
                OnTrackCount = onTrackCount,
                PendingReviewsCount = pendingReviews,
                Projects = projects
            };
        }

        public async Task<Project> CreateProjectAsync(CreateProjectRequest request, string creatorName)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadRequestException("Project name is required.");
            }

            var project = new Project
            {
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Category = string.IsNullOrWhiteSpace(request.Category) ? "RBAC Rollout" : request.Category.Trim(),
                Status = string.IsNullOrWhiteSpace(request.Status) ? "In Progress" : request.Status.Trim(),
                Priority = string.IsNullOrWhiteSpace(request.Priority) ? "Medium" : request.Priority.Trim(),
                LeadName = string.IsNullOrWhiteSpace(request.LeadName) ? creatorName : request.LeadName.Trim(),
                ProgressPercentage = Math.Clamp(request.ProgressPercentage, 0, 100),
                DueDate = string.IsNullOrWhiteSpace(request.DueDate) ? DateTime.UtcNow.AddMonths(1).ToString("MMM dd, yyyy") : request.DueDate.Trim(),
                CreatedAt = DateTime.UtcNow,
                DeletedFlag = 1
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> UpdateProjectAsync(int id, UpdateProjectRequest request)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && p.DeletedFlag == 1);
            if (project == null) return null;

            project.Name = request.Name.Trim();
            project.Description = request.Description.Trim();
            project.Category = request.Category.Trim();
            project.Status = request.Status.Trim();
            project.Priority = request.Priority.Trim();
            project.LeadName = request.LeadName.Trim();
            project.ProgressPercentage = Math.Clamp(request.ProgressPercentage, 0, 100);
            project.DueDate = request.DueDate.Trim();

            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && p.DeletedFlag == 1);
            if (project == null) return false;

            project.DeletedFlag = 0;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
