using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class KucoinClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly string passphrase;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.kucoin.com";

    private long timeOffset = 0;

    public KucoinClient(string apiKey, string secretKey, string passphrase)
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
        httpClient.DefaultRequestHeaders.Add("KC-API-KEY", apiKey);
        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    }

    // 1. Lấy server time từ Kucoin
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v1/timestamp");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.data;
    }

    // 2. Sync thời gian với server Kucoin
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
            throw new Exception("Lỗi sync time với Kucoin server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo chữ ký HMAC SHA256 cho Kucoin
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

    // 4. Tạo chữ ký cho passphrase
    private string CreatePassphraseSignature()
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var passphraseBytes = Encoding.UTF8.GetBytes(passphrase);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(passphraseBytes);
            return Convert.ToBase64String(hash);
        }
    }

    // 5. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        try
        {
            long timestamp = GetTimestamp();
            string timestampStr = timestamp.ToString();

            string requestPath = "/api/v1/accounts";
            string method = "GET";
            string body = "";

            string signature = CreateSignature(timestampStr, method, requestPath, body);
            string passphraseSignature = CreatePassphraseSignature();

            httpClient.DefaultRequestHeaders.Remove("KC-API-SIGN");
            httpClient.DefaultRequestHeaders.Add("KC-API-SIGN", signature);
            httpClient.DefaultRequestHeaders.Remove("KC-API-TIMESTAMP");
            httpClient.DefaultRequestHeaders.Add("KC-API-TIMESTAMP", timestampStr);
            httpClient.DefaultRequestHeaders.Remove("KC-API-PASSPHRASE");
            httpClient.DefaultRequestHeaders.Add("KC-API-PASSPHRASE", passphraseSignature);

            string url = $"{BASE_URL}{requestPath}";
            var res = await httpClient.GetAsync(url);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Kucoin API Error: {res.StatusCode}");
            }

            return await res.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ Kucoin", ex);
        }
    }
}
