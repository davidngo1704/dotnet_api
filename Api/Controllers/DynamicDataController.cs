using Api.Database;
using Api.Database.Models;
using Api.Models.ApplicationModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicDataController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public DynamicDataController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTop1000()
    {
        var data = _db.Humans.Take(1000).ToList();
        return Ok(data);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] HumanEditModel human)
    {
        var data = _db.Humans.FirstOrDefault(m => m.Id == human.Id);

        _mapper.Map(human, data);

        await _db.SaveChangesAsync();

        return Ok(data);

    }
}
