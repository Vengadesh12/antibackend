using MyBackend.Application.Contracts;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface ISettingService
    {
        Task<SettingsOverviewResponse> GetSettingsAsync(string? category, string? search);
        Task<List<SettingCategoryDto>> GetCategoriesAsync();
        Task<SettingCategoryDto> CreateCategoryAsync(CreateSettingCategoryRequest request, string callerName);
        Task<SettingCategoryDto?> UpdateCategoryAsync(int id, UpdateSettingCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> UpdateSettingsBulkAsync(UpdateSettingsBulkRequest request, string callerName);
        Task<SystemSetting> CreateSettingAsync(CreateSettingRequest request, string callerName);
        Task<SystemSetting?> UpdateSettingAsync(int id, UpdateSettingRequest request, string callerName);
        Task<bool> DeleteSettingAsync(int id);
    }
}
