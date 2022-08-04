using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs;
using OrdlyBackend.Interfaces;
using OrdlyBackend.DTOs.v1;
using OrdlyBackend.Models;

namespace OrdlyBackend.Controllers.v1;

[ApiController]
[Route("/api/v1/[controller]")]

public class RankController : ControllerBase
{
    private IRankService _rankService;

    public RankController(IRankService rankService)
    {
        _rankService = rankService;
    }


    [HttpPut]
    public async Task<ActionResult<UserRank>> AddOrUpdateUserRank(UserRank userRank)
    {
        await _rankService.AddOrUpdateUserRankAsync(userRank);
        return Ok(new UserRank() { UserId = userRank.UserId, Rating = 0 });
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