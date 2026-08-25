namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// System and workspace configuration setting key-value entity.
    /// </summary>
    public class SystemSetting
    {
        public int Id { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = "string";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = "System Admin";
    }
}
