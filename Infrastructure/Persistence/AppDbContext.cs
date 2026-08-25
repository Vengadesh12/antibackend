using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ScheduleEvent> Schedules { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<SettingCategory> SettingCategories { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("user_sessions");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.UserName).HasColumnName("user_name");
                entity.Property(e => e.IpAddress).HasColumnName("ip_address");
                entity.Property(e => e.UserAgent).HasColumnName("user_agent");
                entity.Property(e => e.LoginTime).HasColumnName("login_time");
                entity.Property(e => e.LogoutTime).HasColumnName("logout_time");
                entity.Property(e => e.SessionToken).HasColumnName("session_token");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.PermissionKey).HasColumnName("PermissionKey");
                entity.Property(p => p.Name).HasColumnName("Name");
                entity.Property(p => p.Description).HasColumnName("Description");
                entity.Property(p => p.DeletedFlag).HasColumnName("DeletedFlag");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("rolepermissions");
                entity.Property(rp => rp.Id).HasColumnName("Id");
                entity.Property(rp => rp.RoleId).HasColumnName("RoleId");
                entity.Property(rp => rp.PermissionId).HasColumnName("PermissionId");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(user => user.Id).HasColumnName("Id");
                entity.Property(user => user.Name).HasColumnName("Name");
                entity.Property(user => user.Email).HasColumnName("Email");
                entity.Property(user => user.PasswordHash).HasColumnName("Password");
                entity.Property(user => user.RoleId).HasColumnName("RoleId");
                entity.Property(user => user.DesignationId).HasColumnName("DesignationId");
                entity.Property(user => user.Phone).HasColumnName("Phone");
                entity.Property(user => user.Age).HasColumnName("Age");
                entity.Property(user => user.Address).HasColumnName("Address");
                entity.Property(user => user.DeletedFlag).HasColumnName("DeletedFlag");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.Property(role => role.Id).HasColumnName("Id");
                entity.Property(role => role.Name).HasColumnName("Name");
                entity.Property(role => role.Description).HasColumnName("Description");
                entity.Property(role => role.DeletedFlag).HasColumnName("DeletedFlag");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("designations");
                entity.Property(d => d.Id).HasColumnName("Id");
                entity.Property(d => d.Name).HasColumnName("Name");
                entity.Property(d => d.Description).HasColumnName("Description");
                entity.Property(d => d.DeletedFlag).HasColumnName("DeletedFlag");
                entity.Property(d => d.CreatedAt).HasColumnName("CreatedAt");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menus");
                entity.Property(menu => menu.Id).HasColumnName("id");
                entity.Property(menu => menu.MenuKey).HasColumnName("menukey");
                entity.Property(menu => menu.Label).HasColumnName("label");
                entity.Property(menu => menu.Icon).HasColumnName("icon");
                entity.Property(menu => menu.Route).HasColumnName("route");
                entity.Property(menu => menu.GroupName).HasColumnName("groupname");
                entity.Property(menu => menu.Description).HasColumnName("description");
                entity.Property(menu => menu.OrderIndex).HasColumnName("orderindex");
                entity.Property(menu => menu.PermissionKey).HasColumnName("permissionkey");
                entity.Property(menu => menu.DeletedFlag).HasColumnName("deletedflag");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("audit_logs");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Action).HasColumnName("action");
                entity.Property(e => e.Module).HasColumnName("module");
                entity.Property(e => e.PerformedBy).HasColumnName("performed_by");
                entity.Property(e => e.Details).HasColumnName("details");
                entity.Property(e => e.IpAddress).HasColumnName("ip_address");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("reports");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.Format).HasColumnName("format");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.FileSize).HasColumnName("file_size");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("projects");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Priority).HasColumnName("priority");
                entity.Property(e => e.LeadName).HasColumnName("lead_name");
                entity.Property(e => e.ProgressPercentage).HasColumnName("progress_percentage");
                entity.Property(e => e.DueDate).HasColumnName("due_date");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<ScheduleEvent>(entity =>
            {
                entity.ToTable("schedules");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.EventType).HasColumnName("event_type");
                entity.Property(e => e.EventDate).HasColumnName("event_date");
                entity.Property(e => e.StartTime).HasColumnName("start_time");
                entity.Property(e => e.EndTime).HasColumnName("end_time");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.Organizer).HasColumnName("organizer");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Priority).HasColumnName("priority");
                entity.Property(e => e.AttendeesCount).HasColumnName("attendees_count");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.ToTable("system_settings");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SettingKey).HasColumnName("setting_key");
                entity.Property(e => e.SettingValue).HasColumnName("setting_value");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.DataType).HasColumnName("data_type");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<SettingCategory>(entity =>
            {
                entity.ToTable("setting_categories");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Icon).HasColumnName("icon");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("event_types");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Color).HasColumnName("color");
                entity.Property(e => e.Icon).HasColumnName("icon");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.DeletedFlag).HasColumnName("deleted_flag");
            });
        }
    }
}