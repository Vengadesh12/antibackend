namespace MyBackend.Domain.Common
{
    /// <summary>
    /// Base entity class containing common identifier and soft-delete state.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// Soft deletion flag (1 = Active, 0 = Inactive/Deleted).
        /// </summary>
        public int DeletedFlag { get; set; } = 1;
    }
}
