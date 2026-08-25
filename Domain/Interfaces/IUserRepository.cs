using MyBackend.Domain.Entities;

namespace MyBackend.Domain.Interfaces
{
    /// <summary>
    /// Specialized repository contract for User entity operations and authorization queries.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Retrieves a user by normalized email address.
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves all users ordered by ID.
        /// </summary>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a single user by ID.
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Updates the soft-delete flag of a user (1 = Active, 0 = Inactive).
        /// </summary>
        Task<bool> SetDeletedFlagAsync(int id, int deletedFlag);

        /// <summary>
        /// Checks whether a user has any of the specified permission keys.
        /// </summary>
        Task<bool> HasPermissionAsync(int userId, params string[] permissionKeys);

        /// <summary>
        /// Retrieves the list of permission keys assigned to the user.
        /// </summary>
        Task<List<string>> GetUserPermissionKeysAsync(int userId);

        /// <summary>
        /// Directly updates the stored password hash of a user.
        /// </summary>
        Task<bool> UpdatePasswordHashAsync(int userId, string newPasswordHash);
    }
}
