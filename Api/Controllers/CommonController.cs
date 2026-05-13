using Api.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CommonController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var key = _config["ASPNETCORE_ENVIRONMENT"];

            if (key == "Production") {

                var data = FileHelper.ReadFromFile("");
            }

            return Ok(key);
        }
    }
}
