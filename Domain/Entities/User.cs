using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// User account database entity representing a registered system member.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique user identifier.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        /// <example>Alex Morgan</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address used for authentication.
        /// </summary>
        /// <example>alex.morgan@example.com</example>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Stored cryptographic hash of the user password.
        /// </summary>
        [JsonIgnore]
        [Column("Password")]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Plain-text password transient property (used during creation/update).
        /// </summary>
        [NotMapped]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the assigned role.
        /// </summary>
        /// <example>2</example>
        public int? RoleId { get; set; }

        /// <summary>
        /// Identifier of the assigned designation.
        /// </summary>
        /// <example>1</example>
        public int? DesignationId { get; set; }

        /// <summary>
        /// Contact telephone number.
        /// </summary>
        /// <example>+1 (555) 019-2834</example>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// User age.
        /// </summary>
        /// <example>32</example>
        public int Age { get; set; }

        /// <summary>
        /// Physical address.
        /// </summary>
        /// <example>123 Innovation Way, Suite 400</example>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Status flag (1 = Active, 0 = Deactivated/Deleted).
        /// </summary>
        /// <example>1</example>
        public int DeletedFlag { get; set; } = 1;
    }
}