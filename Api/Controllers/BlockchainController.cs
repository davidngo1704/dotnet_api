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
        public async Task<IActionResult> GetBalance()
        {
            string endpoint = "http://192.168.1.9:1704/linux/execute";

            var mexcResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/mexc && source .venv/bin/activate && python balance_spot.py"
            });

            var kucoinResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/kucoin && source .venv/bin/activate && python balance_spot.py"
            });

            var bingxResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/bingx && source .venv/bin/activate && python balance_spot.py"
            });

            var okxResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/okx && source .venv/bin/activate && python balance_spot.py"
            });

            var binanceResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/binance && source .venv/bin/activate && python balance_spot.py"
            });

            var bybitResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/bybit && source .venv/bin/activate && python balance_spot.py"
            });

            var bitgetResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/bitget && source .venv/bin/activate && python balance_spot.py"
            });

            var gateResponse = _httpService.PostAsync<CommonRequest, CommonResponse>(endpoint, new CommonRequest()
            {
                command = "cd /var/lib/ApiGateway/cold_wallet/blockchain/cex/gate && source .venv/bin/activate && python balance_spot.py"
            });

            var data = await Task.WhenAll(
                mexcResponse,
                kucoinResponse,
                bingxResponse,
                okxResponse,
                binanceResponse,
                bybitResponse,
                bitgetResponse,
                gateResponse
            );

            var mexc = JsonSerializer.Deserialize<MexcSpotModels>(data[0]?.data!, DefaultValue.JsonOption);

            var kucoin = JsonSerializer.Deserialize<KucoinBalanceResponse>(data[1]?.data!, DefaultValue.JsonOption);

            var bingx = JsonSerializer.Deserialize<List<CryptoModel>>(data[2]?.data!, DefaultValue.JsonOption);

            var okx = JsonSerializer.Deserialize<List<CryptoModel>>(data[3]?.data!, DefaultValue.JsonOption);

            var binance = JsonSerializer.Deserialize<List<CryptoModel>>(data[4]?.data!, DefaultValue.JsonOption);

            var bybit = JsonSerializer.Deserialize<List<CryptoModel>>(data[5]?.data!, DefaultValue.JsonOption);

            var bitget = JsonSerializer.Deserialize<BitgetModel>(data[6]?.data!, DefaultValue.JsonOption);

            var gate = JsonSerializer.Deserialize<GateSpotModels>(data[7]?.data!, DefaultValue.JsonOption);

            return Ok(new 
            {
                mexc,
                kucoin,
                bingx,
                okx,
                binance,
                bybit,
                bitget,
                gate
            });
        }
    }
}
