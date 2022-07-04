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

        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, null);
    }

    [HttpGet("{id}")]
    [ActionName("GetUserByIdAsync")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        else return Ok(user);
    }

    [HttpGet("latestUser")]
    [ActionName("GetLatestUserByIdAsync")]

    public async Task<ActionResult<User>> GetLatestUserByIdAsync()
    {
        var latestUser = _context.Users.OrderByDescending(x => x.Id).FirstOrDefault();
        return latestUser;
    }
}