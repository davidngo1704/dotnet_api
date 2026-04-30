using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class MexcClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.mexc.com";

    private long timeOffset = 0;

    public MexcClient(string apiKey, string secretKey)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;

        httpClient = new HttpClient();
        SetDefaultHeaders();
    }

    private void SetDefaultHeaders()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-MEXC-APIKEY", apiKey);
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    }

    // 1. Lấy server time từ MEXC
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v3/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.serverTime;
    }

    // 2. Sync thời gian với server MEXC
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
            throw new Exception("Lỗi sync time với MEXC server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo chữ ký HMAC SHA256 cho MEXC
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

    // 4. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        try
        {
            long timestamp = GetTimestamp();
            string timestampStr = timestamp.ToString();

            string query = $"timestamp={timestampStr}&recvWindow=5000";
            string signature = CreateSignature(query);

            httpClient.DefaultRequestHeaders.Remove("X-MEXC-SIGN");
            httpClient.DefaultRequestHeaders.Add("X-MEXC-SIGN", signature);

            string url = $"{BASE_URL}/api/v3/account?{query}&signature={signature}";
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"MEXC API Error: {res.StatusCode}");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ MEXC", ex);
        }
    }
}
