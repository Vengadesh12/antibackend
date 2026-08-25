namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Represents an authorization role assigned to users in the RBAC system.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique role identifier.
        /// </summary>
        /// <example>2</example>
        public int Id { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        /// <example>Super Admin</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the role responsibilities and privileges.
        /// </summary>
        /// <example>Full system access and authority to manage all workspaces and permissions.</example>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Status flag (1 = Active, 0 = Deleted).
        /// </summary>
        /// <example>1</example>
        public int DeletedFlag { get; set; } = 1;
    }
}
