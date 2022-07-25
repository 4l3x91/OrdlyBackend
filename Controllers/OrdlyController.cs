using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Models;
using OrdlyBackend.Services;
using OrdlyBackend.Interfaces;

namespace OrdlyBackend.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]

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
    public async Task<ActionResult<DailyGame>> GetDailyGame()
    {
        var dailyWord = _context.DailyWords.OrderBy(x => x.DailyWordId).Last();
        var word = await _context.Words.FindAsync(dailyWord.WordId);
        var dailyGame = new DailyGame()
        {
            DailyGameId = dailyWord.DailyWordId,
            Word = word.Name
        };

        return Ok(dailyGame);
    }

    [HttpPost("guess")]
    public async Task<ActionResult<GuessResponse>> GetGuessResult([FromBody] GuessRequest request)
    {
        var result = await _gameService.GetGuessResultAsync(request);
        return Ok(result);
    }

    [HttpGet("currentTime")]
    public ActionResult<DateTime> GetTime()
    {
        var time = DateTime.Now;
        return Ok(time);
    }
}
