
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
        [HttpGet]
        public async Task<IActionResult> BingxGetPosition()
        {
            var client = new BingXClient(
                "tl77B8Cu3kD6qOO98qofzJb6M5dbTMl7KTL1ddUHREHCjvXxRp5ARHWq5j9uMGlguNyBNLIHaBYq16kQ",
                "x0XW5WkWjWLxwHV4S2AQ9JlC64rvwgI7IIe0bdzh3qeXqR4lpRd2BakBwTeEIEG0QTPPmI5TfHDg6CtV6DQ"
            );

            var resultString = await client.GetPositions();

            var result = JsonConvert.DeserializeObject<BingxApiResponse>(resultString);

            var finalResult = new List<object>();

            foreach (var item in result?.Data!)
            {
                finalResult.Add(new
                {
                    Symbol = item.Symbol,
                    TaiXiu = item.PositionSide == "LONG" ? "LONG" : "SHORT",
                    LaiLo = item.UnrealizedProfit,
                    GiaThanhLy = item.LiquidationPrice,
                    GiaDanhDau = item.MarkPrice,
                    GiaVaoLenh = item.AvgPrice,
                    DonBay = item.Leverage,
                });
            }

            return Ok(finalResult);
        }
        [HttpGet]
        public async Task<IActionResult> BinanceGetPosition()
        {
            var client = new BinanceClient(
                "06MzlJ1aV3quq7f8WnBPp73iHLpNEkFgGBLTVFmjEJ0W29bLXIVNJ7WUgG64LnYb",
                "rrof6lQsbXMTyeuSVkBbS1WbQWUnJbB7gotZgOND2TyFTmdnAtI5MdEUGUjWZzZI"
            );

            var result = await client.GetSpotBalance();

            var data = JsonConvert.DeserializeObject<AccountInfo>(result);

            var resultReal = new List<Balance>();

            if (data?.Balances != null)
            {
                foreach (var item in data.Balances)
                {
                    if (item.Free > 0)
                    {
                        if (item.Asset != null && item.Asset.StartsWith("LD"))
                        {
                            item.Asset = item.Asset.Replace("LD", "");
                        }

                        if (item.Asset == "USDT")
                        {
                            item.Price = 1;
                            resultReal.Add(item);
                            continue;
                        }

                        var dataPrice = await _httpService.GetAsync<CoinPriceModel>(@$"https://api.binance.com/api/v3/ticker/price?symbol={item.Asset}USDT");

                        item.Price = dataPrice?.price ?? 0;

                        resultReal.Add(item);
                    }
                }
            }

            return Ok(new
            {
                Total = resultReal.Sum(x => x.Free * x.Price),
                Coin = resultReal,
            });
        }
        [HttpGet]
        public async Task<IActionResult> GateGetPosition()
        {
            var client = new GateClient(
                "fbb928647a0b6dceaaafb553f4eff6ba",
                "c13133a956c20e1c0a5e06c60b142717ca59f4c63f5108fef8edd456a7124d44"
            );
            var resultString = await client.GetSpotBalance();

            return Ok(resultString);
        }
    }
}