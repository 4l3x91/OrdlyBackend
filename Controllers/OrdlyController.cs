using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.DTOs.v1;
using OrdlyBackend.Utilities;

namespace OrdlyBackend.Controllers.v1;

[ApiController]
[Route("/api/v1/[controller]")]

public class OrdlyController : ControllerBase
{
    private IGameService _gameService;
    private IDailyWordService _dailyWordService;
    private OrdlySettings _settings;

    public OrdlyController(OrdlyContext context, IGameService gameService, IDailyWordService dailyWordService, OrdlySettings settings)
    {
        _settings = settings;
        _dailyWordService = dailyWordService;
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<DailyGameDTO>> GetDailyGame()
    {
        var lastDailyGame = await _dailyWordService.GetLatestDailyAsync();
        var dailyGame = new DailyGameDTO()
        {
            DailyGameId = lastDailyGame.Id
        };

        return Ok(dailyGame);
    }

    [HttpPost("guess")]
    public async Task<ActionResult<GuessResponse>> GetGuessResult([FromBody] GuessRequest request)
    {
        var result = await _gameService.GetFullGuessResultAsync(request);
        return result != null ? Ok(result) : BadRequest();
    }

    [HttpGet("currentTime")]
    public ActionResult<DateTime> GetTime()
    {
        var time = DateTime.Now;
        return Ok(time);
    }

    [HttpGet("ForceNewDaily")]
    public ActionResult ForceNewWord()
    {
        _settings.ForceNewDaily = true;
        return Ok();
    }
}
