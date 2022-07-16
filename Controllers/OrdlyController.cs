using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<Word>> GetWord()
    {
        var word = _context.Words.OrderBy(x => x.WordId).Last() ;
        return Ok(word);
    }
}