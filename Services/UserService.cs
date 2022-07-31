using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services;

public class UserService : IUserService
{
    IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateUserAsync()
    {
        User user = new()
        {
            UserKey = Guid.NewGuid()
        };
        return await _userRepository.AddAsync(user);
    }

    public async Task<bool> ValidateUserAsync(int userId, Guid userKey)
    {
        var user = await GetUserByIdAsync(userId);
        return user.UserKey == userKey;
    }
        
    public async Task<User> AddOrUpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User> GetLatestUserAsync()
    {
        return await _userRepository.GetLastAsync();
    }
}
