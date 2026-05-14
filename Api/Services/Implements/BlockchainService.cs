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


            var dataXAG = await _httpService.GetAsync<CoinPriceModel>("https://fapi.binance.com/fapi/v1/ticker/price?symbol=XAGUSDT");

            var resultXAG = new CoinPriceResponse()
            {
                data = dataXAG,
                datetime = DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")
            };

            FileHelper.WriteText("/var/lib/ApiGateway/blockchain/cex/binance/priceXAG.json", System.Text.Json.JsonSerializer.Serialize(resultXAG));




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



        }

        public async Task TriggerTenMinute()
        {
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




        }

        public async Task TriggerHour()
        {
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




        }


    }
}
