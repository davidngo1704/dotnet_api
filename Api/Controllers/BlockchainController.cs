
using Api.Libraries;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IHttpService _httpService;

        public BlockchainController(
            IBlockchainService blockchainService,
            IHttpService httpService
            )
        {
            _blockchainService = blockchainService;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> TriggerMinute()
        {
            await _blockchainService.TriggerMinute();
            return Ok(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
       
        [HttpGet]
        public async Task<IActionResult> TriggerTenMinute()
        {
            await _blockchainService.TriggerTenMinute();
            return Ok(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        
        [HttpGet]
        public async Task<IActionResult> TriggerHour()
        {
            await _blockchainService.TriggerHour();
            return Ok(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }
}