using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;


namespace Api.Libraries;

public class BinanceClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.binance.com";

    public BinanceClient(string apiKey, string secretKey)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;

        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
    }

    // 1. Lấy server time
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v3/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.serverTime;
    }
    private long timeOffset = 0;

    public async Task SyncTime()
    {
        var serverTime = await GetServerTime();
        var localTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        timeOffset = serverTime - localTime;
    }
    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }
    // 2. Tạo chữ ký HMAC SHA256
    private string CreateSignature(string queryString)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var queryBytes = Encoding.UTF8.GetBytes(queryString);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(queryBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // 3. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        long timestamp = GetTimestamp();

        string query = $"timestamp={timestamp}&recvWindow=5000";

        string signature = CreateSignature(query);

        string url = $"{BASE_URL}/api/v3/account?{query}&signature={signature}";

        var res = await httpClient.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }

}
