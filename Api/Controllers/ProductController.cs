using Api.Database;
using Api.Database.Models;
using Api.Models.ApplicationModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTop1000(string tableName)
        {
            var data = _db.Products.Take(1000).ToList();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> SearchTop1000(string search, string tableName)
        {
            var queryData = _db.Products.AsQueryable();

            var data = queryData.Where(m =>
            m.Name.Contains(search) ||
            m.Code.Contains(search) ||
            m.Description.Contains(search) ||
            m.Value.Contains(search)
            ).Take(1000).ToList();

            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ProductEditModel obj)
        {
            var data = _db.Products.FirstOrDefault(m => m.Id == obj.Id);

            _mapper.Map(obj, data);

            await _db.SaveChangesAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductAddModel obj)
        {
            var data = _mapper.Map<Product>(obj);

            _db.Products.Add(data);

            await _db.SaveChangesAsync();

            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var data = _db.Products.FirstOrDefault(m => m.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
