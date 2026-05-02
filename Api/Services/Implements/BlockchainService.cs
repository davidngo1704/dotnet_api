using Api.Libraries;
using Api.Models;
using Api.Services.Interfaces;

namespace Api.Services.Implements
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IHttpService _httpService;
        public BlockchainService(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task TriggerMinute()
        {
            var dataBTC = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=WLDUSDT");

            var resultBTC = new CoinPriceResponse()
            {
                data = dataBTC,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceBTC.json", System.Text.Json.JsonSerializer.Serialize(resultBTC));

            //var clientBingx = new BingXClient(
            //    "tl77B8Cu3kD6qOO98qofzJb6M5dbTMl7KTL1ddUHREHCjvXxRp5ARHWq5j9uMGlguNyBNLIHaBYq16kQ",
            //    "x0XW5WkWjWLxwHV4S2AQ9JlC64rvwgI7IIe0bdzh3qeXqR4lpRd2BakBwTeEIEG0QTPPmI5TfHDg6CtV6DQ"
            //);

            //var resultBingx = await clientBingx.GetPositions();

            //FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/bingx/result.json", resultBingx);

            //var clientBinance = new BinanceClient(
            //    "06MzlJ1aV3quq7f8WnBPp73iHLpNEkFgGBLTVFmjEJ0W29bLXIVNJ7WUgG64LnYb",
            //    "rrof6lQsbXMTyeuSVkBbS1WbQWUnJbB7gotZgOND2TyFTmdnAtI5MdEUGUjWZzZI"
            //);

            //var resultBinance = await clientBinance.GetSpotBalance();

            //FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/result.json", resultBinance);


            var dataETH = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=WLDUSDT");

            var resultETH = new CoinPriceResponse()
            {
                data = dataETH,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceETH.json", System.Text.Json.JsonSerializer.Serialize(resultETH));

        }




    }
}
