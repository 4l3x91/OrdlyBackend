using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs.v2;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Models;
using OrdlyBackend.Services;
using OrdlyBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OrdlyBackend.Controllers.v2;

[ApiController]
[Route("/api/v2/[controller]")]

public class OrdlyController : ControllerBase
{
    private OrdlyContext _context;
    private IGameService _gameService;

    public OrdlyController(OrdlyContext context, IGameService gameService)
    {
        _context = context;
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<DailyGame2>> GetDailyGame()
    {
        var dailyWord = await _context.DailyWords.OrderBy(x => x.Id).LastAsync();
        var dailyGame = new DailyGame2()
        {
            DailyGameId = dailyWord.Id
        };

        return Ok(dailyGame);
    }

    [HttpPost("guess")]
    public async Task<ActionResult<GuessResponse2>> GetGuessResult([FromBody] DTOs.GuessRequest request)
    {
        var result = await _gameService.GetFullGuessResultAsync(request);
        return result != null ? Ok(result) : BadRequest();
    }
}
