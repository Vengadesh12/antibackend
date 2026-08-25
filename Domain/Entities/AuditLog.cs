namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Audit log entity recording security, administrative, and data change events.
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string IpAddress { get; set; } = "127.0.0.1";
        public string Status { get; set; } = "Success";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int DeletedFlag { get; set; } = 1;
    }
}
