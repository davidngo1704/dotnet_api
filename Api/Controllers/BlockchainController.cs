
using Api.Libraries;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IBlockchainService _blockchainService;
        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }
        [HttpGet]
        public async Task<IActionResult> TriggerMinute()
        {
            await _blockchainService.TriggerMinute();
            return Ok(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        [HttpGet]
        public async Task<IActionResult> GetPosition()
        {
            var client = new BingXClient("vlFj2CjUeDH270a6of0Z0qgIhep7jgMRjCZmibYogTOecxHeFxokVu3VkOlF4Ru0u2XorIrljEiLc08DmxbTw", "XaBwLx9DnDIKgeKorcJCh0mZyuFSmA4iOojPDwhaGauf9Vi5uGoEwgji3JhGvZalHilJybIc8ZKQZTBchzPfQ");

            var result = await client.GetPositions();
            return Ok(result);
        }
    }
}
