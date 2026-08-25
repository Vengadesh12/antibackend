using MyBackend.Domain.Entities;

namespace MyBackend.Domain.Interfaces
{
    /// <summary>
    /// Specialized repository contract for Role entity queries and operations.
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Retrieves all active workspace roles (DeletedFlag = 1).
        /// </summary>
        Task<List<Role>> GetActiveRolesAsync();

        /// <summary>
        /// Retrieves an active role by ID.
        /// </summary>
        Task<Role?> GetActiveRoleByIdAsync(int id);

        /// <summary>
        /// Updates the soft-delete flag of a role.
        /// </summary>
        Task<bool> SetDeletedFlagAsync(int id, int deletedFlag);

        /// <summary>
        /// Retrieves a lookup dictionary of Role ID to Role Name for active roles.
        /// </summary>
        Task<Dictionary<int, string>> GetRoleNameDictionaryAsync();
    }
}
