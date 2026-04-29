
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
            var client = new BingXClient(
                "tl77B8Cu3kD6qOO98qofzJb6M5dbTMl7KTL1ddUHREHCjvXxRp5ARHWq5j9uMGlguNyBNLIHaBYq16kQ",
                "x0XW5WkWjWLxwHV4S2AQ9JlC64rvwgI7IIe0bdzh3qeXqR4lpRd2BakBwTeEIEG0QTPPmI5TfHDg6CtV6DQ"
            );

            var result = await client.GetPositions();
            return Ok(result);
        }
    }
}
