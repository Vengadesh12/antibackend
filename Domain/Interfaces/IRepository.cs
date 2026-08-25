using System.Linq.Expressions;

namespace MyBackend.Domain.Interfaces
{
    /// <summary>
    /// Generic repository contract providing standard CRUD and query operations for domain entities.
    /// </summary>
    /// <typeparam name="T">The domain entity type.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its integer identifier.
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all entities of type T.
        /// </summary>
        Task<List<T>> ListAllAsync();

        /// <summary>
        /// Retrieves entities matching a filter expression.
        /// </summary>
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves the first entity matching a filter expression or default.
        /// </summary>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks whether any entity matches a filter condition.
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// Returns total count of entities matching a filter condition.
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities to the repository.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Marks an entity as modified.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Removes multiple entities from the repository.
        /// </summary>
        void DeleteRange(IEnumerable<T> entities);
    }
}
