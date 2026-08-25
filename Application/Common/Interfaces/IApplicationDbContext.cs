using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Common.Interfaces
{
    /// <summary>
    /// Database context abstraction for the application layer.
    /// </summary>
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Designation> Designations { get; }
        DbSet<Permission> Permissions { get; }
        DbSet<RolePermission> RolePermissions { get; }
        DbSet<UserSession> UserSessions { get; }
        DbSet<Menu> Menus { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<Report> Reports { get; }
        DbSet<Project> Projects { get; }
        DbSet<ScheduleEvent> Schedules { get; }
        DbSet<SystemSetting> SystemSettings { get; }
        DbSet<SettingCategory> SettingCategories { get; }
        DbSet<EventType> EventTypes { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
