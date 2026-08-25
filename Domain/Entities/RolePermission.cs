namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Join entity mapping assigned permissions to roles.
    /// </summary>
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
