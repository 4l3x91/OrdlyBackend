using Microsoft.AspNetCore.Mvc;
using OrdlyBackend.DTOs;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Models;

namespace OrdlyBackend.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]

public class OrdlyController : ControllerBase
{
    OrdlyContext _context;

    public OrdlyController(OrdlyContext context) => _context = context;

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
}
