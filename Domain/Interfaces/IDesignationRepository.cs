using MyBackend.Domain.Entities;

namespace MyBackend.Domain.Interfaces
{
    /// <summary>
    /// Specialized repository contract for Designation entity queries and operations.
    /// </summary>
    public interface IDesignationRepository : IRepository<Designation>
    {
        /// <summary>
        /// Retrieves all active workspace designations (DeletedFlag = 1).
        /// </summary>
        Task<List<Designation>> GetActiveDesignationsAsync();

        /// <summary>
        /// Retrieves an active designation by ID.
        /// </summary>
        Task<Designation?> GetActiveDesignationByIdAsync(int id);

        /// <summary>
        /// Retrieves a lookup dictionary of Designation ID to Designation Name for active designations.
        /// </summary>
        Task<Dictionary<int, string>> GetDesignationNameDictionaryAsync();
    }
}
