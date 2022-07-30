using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Controllers.v2;

[ApiController]
[Route("/api/v2/[controller]")]

public class UserController : ControllerBase
{
    private IMapper _mapper;
    private IUserService _userService;

    public UserController(IUserService userService, IMapper mapper)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseUserDTO>> GetNewUser()
    {
        var user = await _userService.CreateUserAsync();
        var dto = new BaseUserDTO() { UserId = user.UserId, UserKey = user.UserKey };
        return CreatedAtAction(nameof(GetUserByIdAsync), new { userId = user.UserId }, dto);
    }

    [HttpPut]
    public async Task<ActionResult<BaseUserDTO>> AddOrUpdateUser(User user)
    {
        await _userService.AddOrUpdateUserAsync(user);
        return Ok(new BaseUserDTO() { UserId = user.UserId, UserKey = user.UserKey });
    }

    [HttpGet("{userId}")]
    [ActionName("GetUserByIdAsync")]
    public async Task<ActionResult<BaseUserDTO>> GetUserByIdAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NoContent();
        else return Ok(new BaseUserDTO() { UserId = user.UserId, UserKey = user.UserKey });
    }

    [HttpGet("latestUser")]
    [ActionName("GetLatestUserByIdAsync")]
    public async Task<ActionResult<BaseUserDTO>> GetLatestUserAsync()
    {
        var user = await _userService.GetLatestUserAsync();
        return user != null ? Ok(new BaseUserDTO() { UserId = user.UserId, UserKey = user.UserKey }) : NoContent();
    }

    [HttpPost("validate")]
    public async Task<ActionResult<bool>> ValidateUserAsync(BaseUserDTO user)
    {
        return Ok(await _userService.ValidateUserAsync(user.UserId, user.UserKey));
    }
}