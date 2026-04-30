using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class BitgetClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly string passphrase;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.bitget.com";

    private long timeOffset = 0;

    public BitgetClient(string apiKey, string secretKey, string passphrase)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;
        this.passphrase = passphrase;

        httpClient = new HttpClient();
        SetDefaultHeaders();
    }

    private void SetDefaultHeaders()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Access-Key", apiKey);
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    }

    // 1. Lấy server time từ Bitget
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v2/public/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.data.serverTime;
    }

    // 2. Sync thời gian với server Bitget
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
            throw new Exception("Lỗi sync time với Bitget server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo chữ ký HMAC SHA256 cho Bitget
    private string CreateSignature(string timestamp, string method, string requestPath, string body)
    {
        var prehash = timestamp + method + requestPath + body;
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var preHashBytes = Encoding.UTF8.GetBytes(prehash);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(preHashBytes);
            return Convert.ToBase64String(hash);
        }
    }

    // 4. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        try
        {
            long timestamp = GetTimestamp();
            string timestampStr = timestamp.ToString();

            string requestPath = "/v2/spot/account/assets";
            string method = "GET";
            string body = "";

            string signature = CreateSignature(timestampStr, method, requestPath, body);

            httpClient.DefaultRequestHeaders.Remove("Access-Sign");
            httpClient.DefaultRequestHeaders.Add("Access-Sign", signature);
            httpClient.DefaultRequestHeaders.Remove("Access-Timestamp");
            httpClient.DefaultRequestHeaders.Add("Access-Timestamp", timestampStr);

            string url = $"{BASE_URL}{requestPath}";
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Bitget API Error: {res.StatusCode}");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ Bitget", ex);
        }
    }
}
