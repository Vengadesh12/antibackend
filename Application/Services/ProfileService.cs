using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBackend.Application.Common.Exceptions;
using MyBackend.Application.Common.Interfaces;
using MyBackend.Application.Common.Validators;
using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Entities;

namespace MyBackend.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public ProfileService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileResponse> GetProfileAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedFlag == 1);

            if (user == null)
            {
                throw new NotFoundException("User profile not found or account is deactivated.");
            }

            string roleName = "Member";
            if (user.RoleId.HasValue)
            {
                roleName = await _context.Roles
                    .Where(r => r.Id == user.RoleId && r.DeletedFlag == 1)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync() ?? "Member";
            }

            var permissions = user.RoleId.HasValue
                ? await _context.Database.SqlQueryRaw<string>("""
                    SELECT p."PermissionKey" AS "Value"
                    FROM permissions p
                    INNER JOIN rolepermissions rp ON rp."PermissionId" = p."Id"
                    WHERE rp."RoleId" = {0} AND p."DeletedFlag" = 1
                    """, user.RoleId.Value).ToListAsync()
                : [];

            return new UserProfileResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Age = user.Age,
                Address = user.Address,
                RoleId = user.RoleId,
                RoleName = roleName,
                Permissions = permissions
            };
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.DeletedFlag == 1);
            if (user == null)
            {
                throw new NotFoundException("User profile not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name)) user.Name = request.Name.Trim();
            if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email.Trim();
            if (request.Phone != null) user.Phone = request.Phone.Trim();
            if (request.Age > 0) user.Age = request.Age;
            if (request.Address != null) user.Address = request.Address.Trim();

            await _context.SaveChangesAsync();

            return new UserProfileResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Age = user.Age,
                Address = user.Address,
                RoleId = user.RoleId
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            {
                throw new BadRequestException("Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new BadRequestException("New password is required.");
            }

            var (isValid, errors) = PasswordValidator.Validate(request.NewPassword);
            if (!isValid)
            {
                throw new BadRequestException(errors.Count > 0 ? errors[0] : "New password does not meet strong security requirements.");
            }

            if (!string.Equals(request.NewPassword, request.ConfirmPassword, StringComparison.Ordinal))
            {
                throw new BadRequestException("New password and confirm password do not match.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.DeletedFlag == 1);
            if (user == null)
            {
                throw new NotFoundException("User profile not found.");
            }

            // Verify current password if user has one
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
                if (verify == PasswordVerificationResult.Failed && !string.Equals(user.PasswordHash, request.CurrentPassword, StringComparison.Ordinal))
                {
                    throw new BadRequestException("Current password entered is incorrect.");
                }
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
