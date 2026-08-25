using MyBackend.Domain.Entities;

namespace MyBackend.Application.Contracts
{
    public class UpdateSettingsBulkRequest
    {
        public Dictionary<string, string> Settings { get; set; } = [];
    }

    public class CreateSettingRequest
    {
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = "string";
    }

    public class UpdateSettingRequest
    {
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = "string";
    }

    public class CreateSettingCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Tune";
    }

    public class UpdateSettingCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Tune";
    }

    public class SettingCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Tune";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System Admin";
        public int DeletedFlag { get; set; } = 1;
        public int SettingsCount { get; set; } = 0;
    }

    public class SettingsOverviewResponse
    {
        public string SecurityLevel { get; set; } = "High";
        public int AlertChannels { get; set; } = 3;
        public string SessionTimeout { get; set; } = "24h";
        public int TotalSettings { get; set; } = 0;
        public int TotalCategories { get; set; } = 0;
        public List<SystemSetting> Settings { get; set; } = [];
        public List<SettingCategoryDto> Categories { get; set; } = [];
    }
}
