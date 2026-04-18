using Api.Database;
using Api.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RoleController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = _db.Roles.ToList();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            _db.Roles.Add(role);

            await _db.SaveChangesAsync();


            return Ok("OllamaController is working!");
        }
    }
}
