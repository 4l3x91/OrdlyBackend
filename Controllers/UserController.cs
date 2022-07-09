using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using ApplicationCore.Services;
using Infrastructure.Data;

namespace WebApi.Controllers.V1;

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

        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.UserId }, null);
    }

    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser(User user)
    {
        _context.Users.Update(user);
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
        var latestUser = _context.Users.OrderByDescending(x => x.UserId).FirstOrDefault();
        return latestUser;
    }
}