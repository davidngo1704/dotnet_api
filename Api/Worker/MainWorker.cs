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



            var dataETH = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT");

            var resultETH = new CoinPriceResponse()
            {
                data = dataETH,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceETH.json", System.Text.Json.JsonSerializer.Serialize(resultETH));



            var dataPAXG = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=PAXGUSDT");

            var resultPAXG = new CoinPriceResponse()
            {
                data = dataPAXG,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/pricePAXG.json", System.Text.Json.JsonSerializer.Serialize(resultPAXG));


            var dataARB = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=ARBUSDT");

            var resultARB = new CoinPriceResponse()
            {
                data = dataARB,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceARB.json", System.Text.Json.JsonSerializer.Serialize(resultARB));

            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataSTRK = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=STRKUSDT");

            var resultSTRK = new CoinPriceResponse()
            {
                data = dataSTRK,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceSTRK.json", System.Text.Json.JsonSerializer.Serialize(resultSTRK));


            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataOP = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=OPUSDT");

            var resultOP = new CoinPriceResponse()
            {
                data = dataOP,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceOP.json", System.Text.Json.JsonSerializer.Serialize(resultOP));

            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataWLD = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=WLDUSDT");

            var resultWLD = new CoinPriceResponse()
            {
                data = dataWLD,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceWLD.json", System.Text.Json.JsonSerializer.Serialize(resultWLD));

            var dataSOL = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=SOLUSDT");

            var resultSOL = new CoinPriceResponse()
            {
                data = dataSOL,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceSOL.json", System.Text.Json.JsonSerializer.Serialize(resultSOL));

            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataBNB = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=BNBUSDT");

            var resultBNB = new CoinPriceResponse()
            {
                data = dataBNB,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceBNB.json", System.Text.Json.JsonSerializer.Serialize(resultBNB));


            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataONDO = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=ONDOUSDT");

            var resultONDO = new CoinPriceResponse()
            {
                data = dataONDO,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceONDO.json", System.Text.Json.JsonSerializer.Serialize(resultONDO));
            //---------------------------------------------------------------------------------------------------------------------------------------------

            var dataLINK = await _httpService.GetAsync<CoinPriceModel>("https://api.binance.com/api/v3/ticker/price?symbol=LINKUSDT");

            var resultLINK = new CoinPriceResponse()
            {
                data = dataLINK,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceLINK.json", System.Text.Json.JsonSerializer.Serialize(resultLINK));






            await Task.Delay(1000, stoppingToken);
        }
    }
}
