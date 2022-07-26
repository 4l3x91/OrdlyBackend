using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services;

public class UserService : IUserService
{
    OrdlyContext _context;

    public UserService(OrdlyContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync()
    {
        User user = new()
        {
            UserKey = Guid.NewGuid()
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> AddOrUpdateUserAsync(User user)
    {
        if (_context.Users.Any(e => e.UserId == user.UserId)) _context.Entry(user).State = EntityState.Modified;
        else _context.Entry(user).State = EntityState.Added;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user;
    }

    public async Task<User> GetLatestUserByIdAsync()
    {
        var latestUser = await _context.Users.OrderByDescending(x => x.UserId).FirstOrDefaultAsync();
        return latestUser;
    }
}
