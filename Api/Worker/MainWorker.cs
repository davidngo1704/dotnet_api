using Api.Libraries;
using Api.Models;

namespace Api.Worker;

public class MinuteWorker : BackgroundService
{
    private readonly IHttpService _httpService;
    public MinuteWorker(IHttpService httpService)
    {
        _httpService = httpService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var dataBTC = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT");

            var resultBTC = new CoinPriceResponse()
            {
                data = dataBTC,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceBTC.json", System.Text.Json.JsonSerializer.Serialize(resultBTC));





            await Task.Delay(1000, stoppingToken);
        }
    }
}
