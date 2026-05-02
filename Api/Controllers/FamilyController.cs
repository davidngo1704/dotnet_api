using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var wife = 1;
            var husband = 2;
            var con1 = 3;
            var con2 = 4;
            var con3 = 5;

            return Ok("ok");
        }
    }
}
