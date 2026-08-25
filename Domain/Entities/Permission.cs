namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// System permission entity defining a specific platform capability.
    /// </summary>
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DeletedFlag { get; set; } = 1;
    }
}
