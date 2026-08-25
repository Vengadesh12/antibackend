namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Workspace RBAC rollout, DevOps, and Governance initiative project entity.
    /// </summary>
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "RBAC Rollout";
        public string Status { get; set; } = "In Progress";
        public string Priority { get; set; } = "Medium";
        public string LeadName { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; } = 0;
        public string DueDate { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int DeletedFlag { get; set; } = 1;
    }
}
