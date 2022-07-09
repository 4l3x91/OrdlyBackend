using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using ApplicationCore.Services;
using Infrastructure.Data;

namespace WebApi.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]

public class OrdlyController : ControllerBase
{
    OrdlyContext _context;

    public OrdlyController(OrdlyContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<Word>> GetWord()
    {
        var TodaysDate = DateTime.Now.ToString("yyyy-MM-dd");
        var word = _context.Words.Where(x => x.Date == TodaysDate);
        return Ok(word);
    }
}