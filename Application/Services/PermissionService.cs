using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Interfaces;

namespace MyBackend.Application.Services
{
    /// <summary>
    /// Implements RBAC permission catalog retrieval and role-permission matrix assignments using repositories.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PermissionsMatrixResponse> GetPermissionsMatrixAsync()
        {
            return await _unitOfWork.Permissions.GetPermissionsMatrixAsync();
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            return await _unitOfWork.Permissions.GetAllActivePermissionsAsync();
        }

        public async Task<List<string>> GetRolePermissionsAsync(int roleId)
        {
            return await _unitOfWork.Permissions.GetPermissionKeysByRoleIdAsync(roleId);
        }

        public async Task<bool> UpdateRolePermissionsAsync(int roleId, UpdatePermissionsRequest request)
        {
            return await _unitOfWork.Permissions.UpdateRolePermissionsAsync(roleId, request.PermissionKeys);
        }
    }
}
