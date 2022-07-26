using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Controllers.v2;

[ApiController]
[Route("/api/v2/[controller]")]

public class UserController : ControllerBase
{
    private OrdlyContext _context;
    private IUserService _userService;

    public UserController(OrdlyContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var newUser = await _userService.CreateUserAsync();

        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = newUser.UserId }, newUser);
    }

    [HttpPut]
    public async Task<ActionResult<User>> AddOrUpdateUser(User user)
    {
        if (_context.Users.Any(e => e.UserId == user.UserId)) _context.Entry(user).State = EntityState.Modified;
        else _context.Entry(user).State = EntityState.Added;

        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpGet("{userId}")]
    [ActionName("GetUserByIdAsync")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();
        else return Ok(user);
    }

    [HttpGet("latestUser")]
    [ActionName("GetLatestUserByIdAsync")]
    public async Task<ActionResult<User>> GetLatestUserByIdAsync()
    {
        var latestUser = await _context.Users.OrderByDescending(x => x.UserId).FirstOrDefaultAsync();
        return latestUser;
    }
}