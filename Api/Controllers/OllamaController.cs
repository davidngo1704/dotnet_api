using Api.Database;
using Api.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OllamaController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OllamaController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = _db.Users.ToList();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            _db.Users.Add(user);

            await _db.SaveChangesAsync();


            return Ok("OllamaController is working!");
        }
    }
}
