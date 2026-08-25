using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Services
{
    public class MenuService : IMenuService
    {
        private const int SuperAdminRoleId = 2;
        private readonly IApplicationDbContext _context;

        public MenuService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetUserMenusAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(item => item.Id == userId && item.DeletedFlag == 1)
                .Select(item => new { item.Id, item.RoleId })
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return [];
            }

            if (user.RoleId == SuperAdminRoleId)
            {
                // Super Admin has access to all active menus
                return await _context.Menus
                    .AsNoTracking()
                    .Where(m => m.DeletedFlag == 1)
                    .OrderBy(m => m.OrderIndex)
                    .ThenBy(m => m.Id)
                    .ToListAsync();
            }
            else if (user.RoleId.HasValue)
            {
                // Get role permissions for regular role
                var allowedPermissionKeys = await _context.Database.SqlQueryRaw<string>("""
                    SELECT p."PermissionKey" AS "Value"
                    FROM permissions p
                    INNER JOIN rolepermissions rp ON rp."PermissionId" = p."Id"
                    WHERE rp."RoleId" = {0} AND p."DeletedFlag" = 1
                    """, user.RoleId.Value).ToListAsync();

                return await _context.Menus
                    .AsNoTracking()
                    .Where(m => m.DeletedFlag == 1 &&
                               (string.IsNullOrEmpty(m.PermissionKey) || allowedPermissionKeys.Contains(m.PermissionKey)))
                    .OrderBy(m => m.OrderIndex)
                    .ThenBy(m => m.Id)
                    .ToListAsync();
            }
            else
            {
                // User has no role: only menus without specific permission requirement
                return await _context.Menus
                    .AsNoTracking()
                    .Where(m => m.DeletedFlag == 1 && string.IsNullOrEmpty(m.PermissionKey))
                    .OrderBy(m => m.OrderIndex)
                    .ThenBy(m => m.Id)
                    .ToListAsync();
            }
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
                .AsNoTracking()
                .Where(m => m.DeletedFlag == 1)
                .OrderBy(m => m.OrderIndex)
                .ThenBy(m => m.Id)
                .ToListAsync();
        }
    }
}
