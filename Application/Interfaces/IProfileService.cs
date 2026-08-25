using MyBackend.Application.Contracts;

namespace MyBackend.Application.Interfaces
{
    public interface IProfileService
    {
        Task<UserProfileResponse> GetProfileAsync(int userId);
        Task<UserProfileResponse> UpdateProfileAsync(int userId, UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
    }
}
