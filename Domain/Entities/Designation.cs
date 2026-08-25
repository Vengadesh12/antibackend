using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Represents an organizational job title / designation entity.
    /// </summary>
    [Table("designations")]
    public class Designation
    {
        /// <summary>
        /// Unique designation identifier.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Name / Title of the designation.
        /// </summary>
        /// <example>Software Engineer</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Short description of responsibilities for this designation.
        /// </summary>
        /// <example>Develops and maintains core applications and services.</example>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Active status flag (1 = Active, 0 = Deactivated).
        /// </summary>
        /// <example>1</example>
        public int DeletedFlag { get; set; } = 1;

        /// <summary>
        /// Timestamp when the designation record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
