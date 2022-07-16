using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Models;

namespace OrdlyBackend.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]

public class UserController : ControllerBase
{
    OrdlyContext _context;

    public UserController(OrdlyContext context) => _context = context;

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.UserId }, user);
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