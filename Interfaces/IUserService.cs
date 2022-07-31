using OrdlyBackend.DTOs;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IUserService
    {
        Task<User> AddOrUpdateUserAsync(User user);
        Task<User> CreateUserAsync();
        Task<User> GetLatestUserAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> ValidateUserAsync(int userId, Guid userKey);
    }
}
