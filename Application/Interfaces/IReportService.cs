using MyBackend.Application.Contracts;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportsOverviewResponse> GetReportsAsync(string? category, string? search);
        Task<List<string>> GetCategoriesAsync();
        Task<ReportDownloadResult?> GetReportDownloadAsync(int id);
        Task<Report> CreateReportAsync(CreateReportRequest request, string creatorName);
        Task<Report?> UpdateReportAsync(int id, UpdateReportRequest request);
        Task<bool> DeleteReportAsync(int id);
    }
}
