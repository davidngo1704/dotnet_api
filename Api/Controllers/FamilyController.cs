using Api.Database;
using Api.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FamilyController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Init()
        {
            var wife = new Human()
            {
                ChiTieu = 10100100,
                Salary = 20200200,
            };
            var husband = new Human()
            {
                ChiTieu = 10100100,
                Salary = 11020020,
                Email = "davidngo1704@gmal.com",
                FacebookLink = "https://www.facebook.com/ngothanhdai1998/",
                FullName = "Ngô Thành Đại",
                Info = "Đại là một người đàn ông có ngoại hình ưa nhìn, với mái tóc đen dày và đôi mắt sáng. Anh ta thường mặc những bộ quần áo lịch sự và có phong cách thời trang riêng. Đại có nụ cười tươi và luôn tạo cảm giác thân thiện khi tiếp xúc với người khác.",
                Password = "Davidkmhd!1",
                PhoneNumber = "0965001740",
                UserName = "davidngo1704",
                WebSiteLink = "https://thanhdai1704.web.app/#/",
            };;
            var con1 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
            };
            var con2 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
            };
            var con3 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
            };

            _db.Humans.Add(wife);
            _db.Humans.Add(husband);
            _db.Humans.Add(con1);
            _db.Humans.Add(con2);
            _db.Humans.Add(con3);

            await _db.SaveChangesAsync();

            return Ok("ok");
        }
    }
}
