using Api.Database;
using Api.Database.Models;
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
        private readonly AppDbContext _db;
        public BlockchainController(IHttpService httpService, AppDbContext db)
        {
            _httpService = httpService;
            _db = db;
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

            var result = new
            {
                mexc,
                kucoin,
                bingx,
                okx,
                binance,
                bybit,
                bitget,
                gate
            };

            var validCoin = new List<string>() {
                "USDT",
                "BTC",
                "ETH",
                "NEAR",
                "TIA",
                "WLD",
                "ARB",
                "ROBO",
                "FLOW",
                "HYPE",
                "ONDO",
                "FET",
                "TAO",
                "SUI",
                "PAXG",
                "SOL",
                "BNB",
                "XRP"
            };

            var resultData = new List<CoinModel>();

            foreach (var item in result.mexc?.balances!)
            {
                if(validCoin.Contains(item.asset!))
                {
                    resultData.Add(new CoinModel()
                    {
                        asset = item.asset,
                        total = item.available
                    });
                }
            }

            foreach (var item in result.kucoin?.balances!)
            {
                if (validCoin.Contains(item.currency!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.currency);
                    if(exist != null)
                    {
                        exist.total += item.available;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.currency,
                            total = item.available
                        });
                    }

                }
            }

            foreach (var item in result.bingx!)
            {
                if (validCoin.Contains(item.asset!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.asset);

                    if (exist != null)
                    {
                        exist.total += item.total;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.asset,
                            total = item.total
                        });
                    }
                }
            }

            foreach (var item in result.okx!)
            {
                if (validCoin.Contains(item.asset!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.asset);

                    if (exist != null)
                    {
                        exist.total += item.total;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.asset,
                            total = item.total
                        });
                    }
                }
            }

            foreach (var item in result.binance!)
            {
                item.asset = item.asset?.Replace("LD", "");

                if (validCoin.Contains(item.asset!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.asset);

                    if (exist != null)
                    {
                        exist.total += item.total;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.asset,
                            total = item.total
                        });
                    }
                }
            }

            foreach (var item in result.bybit!)
            {
                if (validCoin.Contains(item.asset!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.asset);

                    if (exist != null)
                    {
                        exist.total += item.total;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.asset,
                            total = item.total
                        });
                    }
                }
            }

            foreach (var item in result.bitget?.data!)
            {
                if (validCoin.Contains(item.coin!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.coin);

                    if (exist != null)
                    {
                        exist.total += item.available;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.coin,
                            total = item.available
                        });
                    }
                }
            }

            foreach (var item in result.gate?.balances!)
            {
                if (validCoin.Contains(item.asset!))
                {
                    var exist = resultData.FirstOrDefault(x => x.asset == item.asset);

                    if (exist != null)
                    {
                        exist.total += item.free;
                    }
                    else
                    {
                        resultData.Add(new CoinModel()
                        {
                            asset = item.asset,
                            total = item.free
                        });
                    }
                }
            }

            foreach (var item in resultData)
            {
                if(item.asset == "USDT")
                {
                    item.price = 1;
                    item.value = item.total * item.price;
                    continue;
                }

                var binancePrice = await _httpService.GetAsync<CoinPriceModel>($"https://api.binance.com/api/v3/ticker/price?symbol={item.asset}USDT");

                if (binancePrice != null)
                {
                    item.price = binancePrice.price;

                    item.value = item.total * item.price;
                }
            }

            resultData = resultData.Where(x => x.value > 1).ToList();

            return Ok(resultData);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = _db.TargetSymbols.ToList();

            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TargetSymbol user)
        {
            _db.TargetSymbols.Add(user);

            await _db.SaveChangesAsync();

            return Ok("OllamaController is working!");
        }
    }
}
