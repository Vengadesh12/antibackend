using MyBackend.Domain.Entities;

namespace MyBackend.Application.Contracts
{
    public class CreateAuditLogRequest
    {
        public string Action { get; set; } = string.Empty;
        public string Module { get; set; } = "System";
        public string Details { get; set; } = string.Empty;
        public string Status { get; set; } = "Success";
        public string? IpAddress { get; set; }
    }

    public class AuditLogOverviewResponse
    {
        public int TotalEvents { get; set; }
        public int SuccessfulLogins { get; set; }
        public int PrivilegeChanges { get; set; }
        public List<AuditLog> Logs { get; set; } = [];
    }
}
