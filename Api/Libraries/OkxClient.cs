using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class OkxClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly string passphrase;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://www.okx.com";

    private long timeOffset = 0;

    public OkxClient(string apiKey, string secretKey, string passphrase)
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
        httpClient.DefaultRequestHeaders.Add("OK-ACCESS-KEY", apiKey);
        httpClient.DefaultRequestHeaders.Add("OK-ACCESS-PASSPHRASE", passphrase);
    }

    // 1. Lấy server time từ OKX
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v5/public/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.data[0].ts;
    }

    // 2. Sync thời gian với server OKX
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
            throw new Exception("Lỗi sync time với OKX server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo chữ ký HMAC SHA256 cho OKX
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
            string timestampStr = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            string requestPath = "/api/v5/account/balance";
            string method = "GET";
            string body = "";

            string signature = CreateSignature(timestampStr, method, requestPath, body);

            httpClient.DefaultRequestHeaders.Remove("OK-ACCESS-SIGN");
            httpClient.DefaultRequestHeaders.Add("OK-ACCESS-SIGN", signature);
            httpClient.DefaultRequestHeaders.Remove("OK-ACCESS-TIMESTAMP");
            httpClient.DefaultRequestHeaders.Add("OK-ACCESS-TIMESTAMP", timestampStr);

            string url = $"{BASE_URL}{requestPath}";
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"OKX API Error: {res.StatusCode}");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ OKX", ex);
        }
    }
}
