namespace MyBackend.Domain.Entities
{
    /// <summary>
    /// Represents a dynamic navigation menu item in the application.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Unique menu identifier.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Unique menu key string identifier.
        /// </summary>
        /// <example>dashboard.view</example>
        public string MenuKey { get; set; } = string.Empty;

        /// <summary>
        /// Navigation menu display label.
        /// </summary>
        /// <example>Dashboard</example>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Icon symbol or icon identifier.
        /// </summary>
        /// <example>◫</example>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// Client-side application route path.
        /// </summary>
        /// <example>/dashboard</example>
        public string Route { get; set; } = string.Empty;

        /// <summary>
        /// Navigation grouping section name.
        /// </summary>
        /// <example>Core Access</example>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the menu section and functionality.
        /// </summary>
        /// <example>System metrics &amp; access summary</example>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Sorting and display order index.
        /// </summary>
        /// <example>1</example>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Required permission key to view/access this menu (null if public to all authenticated users).
        /// </summary>
        /// <example>dashboard.view</example>
        public string? PermissionKey { get; set; }

        /// <summary>
        /// Status flag (1 = Active, 0 = Deleted).
        /// </summary>
        /// <example>1</example>
        public int DeletedFlag { get; set; } = 1;
    }
}
