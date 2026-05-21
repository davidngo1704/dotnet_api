using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class BingXClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://open-api.bingx.com";

    public BingXClient(string apiKey, string secretKey)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-BX-APIKEY", apiKey);
    }

    // 1. Lấy server time
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/openApi/swap/v2/server/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.data.serverTime;
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

    // 3. Lấy vị thế futures
    public async Task<string> GetPositions(string symbol = "")
    {
        long timestamp = await GetServerTime();

        string query = $"timestamp={timestamp}";
        if (!string.IsNullOrEmpty(symbol))
        {
            query += $"&symbol={symbol}";
        }

        string signature = CreateSignature(query);
        string url = $"{BASE_URL}/openApi/swap/v2/user/positions?{query}&signature={signature}";

        var res = await httpClient.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }

    // 4. Đóng vị thế futures
    public async Task<string> ClosePosition(string positionId, string symbol)
    {
        if (string.IsNullOrEmpty(positionId))
            throw new ArgumentException("Position ID cannot be empty", nameof(positionId));

        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentException("Symbol cannot be empty", nameof(symbol));

        long timestamp = await GetServerTime();

        // Tạo request body
        var requestBody = new
        {
            positionId = positionId,
            symbol = symbol,
            timestamp = timestamp
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);
        string signature = CreateSignature(jsonBody);

        // Tạo URL với signature
        string url = $"{BASE_URL}/openApi/swap/v2/user/positions/close?signature={signature}";

        // Gửi POST request
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var res = await httpClient.PostAsync(url, content);

        return await res.Content.ReadAsStringAsync();
    }
}