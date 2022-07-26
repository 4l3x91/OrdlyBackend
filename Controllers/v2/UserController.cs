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
    public async Task<ActionResult<User>> PostUser()
    {
        var user = await _userService.CreateUserAsync();

        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.UserId }, user);
    }

    [HttpPut]
    public async Task<ActionResult<User>> AddOrUpdateUser(User user)
    {
        var updatedUser = await _userService.AddOrUpdateUserAsync(user);
        return Ok(updatedUser);
    }

    [HttpGet("{userId}")]
    [ActionName("GetUserByIdAsync")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NoContent();
        else return Ok(user);
    }

    [HttpGet("latestUser")]
    [ActionName("GetLatestUserByIdAsync")]
    public async Task<ActionResult<User>> GetLatestUserByIdAsync()
    {
        var latestUser = await _userService.GetLatestUserByIdAsync();
        return latestUser;
    }
}