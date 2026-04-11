using Api.Libraries;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IHttpService _httpService;
        public BlockchainController(IHttpService httpService)
        {
            _httpService = httpService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpService.PostAsync<CommonRequest, CommonResponse>("http://192.168.1.9:1704/linux/execute", new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/mexc && source .venv/bin/activate && python balance_spot.py"
            });

            var data = JsonSerializer.Deserialize <MexcSpotModels>(response?.data!, DefaultValue.JsonOption);

            return Ok(data.Balances);
        }
    }
}
