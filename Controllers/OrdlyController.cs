using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using ApplicationCore.Services;

namespace WebApi.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]

public class OrdlyController : ControllerBase
{
    // OrdlyContext _context;

    // public OrdlyController()
    // {
    //     _context = context;
    // }

    [HttpGet]
    public async Task<ActionResult<Word>> GetWord()
    {
        var word = new { id = "1", name = "fläck" };
        return Ok(word);
    }
}