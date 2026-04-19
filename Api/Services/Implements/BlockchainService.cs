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
                datetime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/price.json", System.Text.Json.JsonSerializer.Serialize(result));
        }




    }
}
