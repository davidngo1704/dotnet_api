using Api.Database;
using Api.Database.Models;
using Api.Models.ApplicationModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FamilyController(AppDbContext db, IMapper mapper)
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

        [HttpGet]
        public async Task<IActionResult> Init()
        {
            var existItem = _db.Humans.FirstOrDefault(m => m.Email == "davidngo1704@gmal.com");

            if (existItem != null)
            {
                return Ok();
            }

            var wife = new Human()
            {
                Email = "Wife",
                FullName = "Wife",
                Info = "Wife",
                UserName = "Wife",
                Password = "WifeOfNgoThanhDai@1998",
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
            };
            var mother = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
                Password = "MotherOfNgoThanhDai@1998",
                Email = "Mother",
                FullName = "Nguyễn Thị Vui",
                Info = "Mother",
                UserName = "VuiNguyen1976",
                PhoneNumber = "0949164429",
                FacebookLink = "https://www.facebook.com/vui.nguyen.334512"
            };
            var con1 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
                Password = "ConOneOfNgoThanhDai@1998",
                Email = "Con1",
                FullName = "Ngô Thành Lộc",
                Info = "Con1",
                UserName = "Con1",
            };
            var con2 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
                Password = "ConTwoOfNgoThanhDai@1998",
                Email = "Con2",
                FullName = "Ngô Thành Lực",
                Info = "Con2",
                UserName = "Con2",
            };
            var con3 = new Human()
            {
                ChiTieu = 5001001,
                Salary = 5001001,
                Password = "ConThreeOfNgoThanhDai@1998",
                Email = "Con3",
                FullName = "Ngô Khả Vi",
                Info = "Con3",
                UserName = "Con3",
            };

            _db.Humans.Add(wife);
            _db.Humans.Add(husband);
            _db.Humans.Add(con1);
            _db.Humans.Add(con2);
            _db.Humans.Add(con3);

            await _db.SaveChangesAsync();

            return Ok("ok");
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
}