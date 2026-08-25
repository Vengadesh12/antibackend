using MyBackend.Application.Contracts;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLogOverviewResponse> GetAuditLogsAsync(string? module, string? search);
        Task<AuditLog> CreateAuditLogAsync(CreateAuditLogRequest request, string performedBy, string ipAddress);
        Task<bool> DeleteAuditLogAsync(int id);
    }
}
