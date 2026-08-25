using MyBackend.Application.Contracts;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface IScheduleService
    {
        Task<SchedulesOverviewResponse> GetSchedulesAsync(string? eventType, string? search);
        Task<ScheduleEvent> CreateScheduleAsync(CreateScheduleRequest request, string creatorName);
        Task<ScheduleEvent?> UpdateScheduleAsync(int id, UpdateScheduleRequest request);
        Task<bool> DeleteScheduleAsync(int id);

        Task<List<EventTypeDto>> GetEventTypesAsync();
        Task<EventTypeDto> CreateEventTypeAsync(CreateEventTypeRequest request, string creatorName);
        Task<bool> DeleteEventTypeAsync(int id);
    }
}
