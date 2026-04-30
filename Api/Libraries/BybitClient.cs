using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class BybitClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.bybit.com";
    private const string RECV_WINDOW = "5000";

    private long timeOffset = 0;

    public BybitClient(string apiKey, string secretKey)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;

        httpClient = new HttpClient();
        SetDefaultHeaders();
    }

    private void SetDefaultHeaders()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-BAPI-KEY", apiKey);
        httpClient.DefaultRequestHeaders.Add("X-BAPI-SIGN-TYPE", "2");
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    }

    // 1. Lấy server time từ Bybit
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/v5/market/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.result.timeSecond * 1000; // Convert seconds to milliseconds
    }

    // 2. Sync thời gian với server Bybit
    public async Task SyncTime()
    {
        try
        {
            var serverTime = await GetServerTime();
            var localTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            timeOffset = serverTime - localTime;
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi sync time với Bybit server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo chữ ký HMAC SHA256 cho Bybit
    private string CreateSignature(string timestamp, string body)
    {
        var prehash = timestamp + apiKey + RECV_WINDOW + body;
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var preHashBytes = Encoding.UTF8.GetBytes(prehash);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(preHashBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // 4. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        try
        {
            long timestamp = GetTimestamp();
            string timestampStr = timestamp.ToString();
            string body = "";

            string signature = CreateSignature(timestampStr, body);

            httpClient.DefaultRequestHeaders.Remove("X-BAPI-SIGN");
            httpClient.DefaultRequestHeaders.Add("X-BAPI-SIGN", signature);
            httpClient.DefaultRequestHeaders.Remove("X-BAPI-TIMESTAMP");
            httpClient.DefaultRequestHeaders.Add("X-BAPI-TIMESTAMP", timestampStr);
            httpClient.DefaultRequestHeaders.Remove("X-BAPI-RECV-WINDOW");
            httpClient.DefaultRequestHeaders.Add("X-BAPI-RECV-WINDOW", RECV_WINDOW);

            string url = $"{BASE_URL}/v5/account/wallet-balance?accountType=SPOT";
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Bybit API Error: {res.StatusCode}");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ Bybit", ex);
        }
    }
}
