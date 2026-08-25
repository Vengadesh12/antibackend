using MyBackend.Domain.Entities;

namespace MyBackend.Application.Contracts
{
    public class CreateReportRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "Compliance";
        public string Format { get; set; } = "PDF";
    }

    public class UpdateReportRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "Compliance";
        public string Format { get; set; } = "PDF";
        public string Status { get; set; } = "Generated";
    }

    public class ReportsOverviewResponse
    {
        public int ReportsGenerated { get; set; }
        public int ExportsReady { get; set; }
        public string RoleCoverage { get; set; } = "100%";
        public List<Report> Reports { get; set; } = [];
    }

    public class ReportDownloadResult
    {
        public byte[] FileBytes { get; set; } = [];
        public string ContentType { get; set; } = "application/octet-stream";
        public string FileName { get; set; } = "report.txt";
    }
}
