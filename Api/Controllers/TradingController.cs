using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TradingController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVersion()
        {
            return Ok("1.0.0");
        }
    }
}
