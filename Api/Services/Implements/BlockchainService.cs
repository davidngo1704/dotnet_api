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
            var data = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=WLDUSDT");

            var result = new CoinPriceResponse()
            {
                data = data,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/price.json", System.Text.Json.JsonSerializer.Serialize(result));

            var client = new BingXClient(
                "tl77B8Cu3kD6qOO98qofzJb6M5dbTMl7KTL1ddUHREHCjvXxRp5ARHWq5j9uMGlguNyBNLIHaBYq16kQ",
                "x0XW5WkWjWLxwHV4S2AQ9JlC64rvwgI7IIe0bdzh3qeXqR4lpRd2BakBwTeEIEG0QTPPmI5TfHDg6CtV6DQ"
            );

            var resultBingx = await client.GetPositions();

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/bingx/result.json", System.Text.Json.JsonSerializer.Serialize(resultBingx));



        }




    }
}
