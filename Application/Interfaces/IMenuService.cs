using MyBackend.Domain.Entities;

namespace MyBackend.Application.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetUserMenusAsync(int userId);
        Task<List<Menu>> GetAllMenusAsync();
    }
}
