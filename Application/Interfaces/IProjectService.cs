using MyBackend.Application.Contracts;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectsOverviewResponse> GetProjectsAsync(string? category, string? status, string? search);
        Task<Project> CreateProjectAsync(CreateProjectRequest request, string creatorName);
        Task<Project?> UpdateProjectAsync(int id, UpdateProjectRequest request);
        Task<bool> DeleteProjectAsync(int id);
    }
}
