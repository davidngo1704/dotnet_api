
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmbeddingVectorController : ControllerBase
    {
        public EmbeddingVectorController()
        {
            
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmbeddingVector obj)
        {
            
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EmbeddingVector obj)
        {

            return Ok();
        }


    }
}
