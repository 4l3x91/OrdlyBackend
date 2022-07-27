using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Models;
using OrdlyBackend.Services;
using OrdlyBackend.Interfaces;

namespace OrdlyBackend.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]

public class RankController : ControllerBase
{
    private OrdlyContext _context;
    private IRankService _rankService;

    public RankController(OrdlyContext context, IRankService rankService)
    {
        _context = context;
        _rankService = rankService;
    }

    [HttpPost("addRating")]
    public async Task<ActionResult<UserRankDTO>> AddRatingAsync(BaseUserDTO user)
    {
        var response = await _rankService.AddRatingAsync(user);
        return response == null ? NoContent() : Ok(response);
    }

    [HttpPost("subtractRating")]
    public async Task<ActionResult<UserRankDTO>> SubtractRatingAsync(BaseUserDTO user)
    {
        var response = await _rankService.SubtractRatingAsync(user);
        return response == null ? NoContent() : Ok(response);
    }
    
}