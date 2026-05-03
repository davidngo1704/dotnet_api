using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Api.Libraries;

public class GateClient
{
    private readonly string apiKey;
    private readonly string secretKey;
    private readonly HttpClient httpClient;

    private const string BASE_URL = "https://api.gateio.ws";

    private long timeOffset = 0;

    public GateClient(string apiKey, string secretKey)
    {
        this.apiKey = apiKey;
        this.secretKey = secretKey;

        httpClient = new HttpClient();
        SetDefaultHeaders();
    }

    private void SetDefaultHeaders()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-Gate-Access-Key", apiKey);
    }

    // 1. Lấy server time từ Gate.io
    public async Task<long> GetServerTime()
    {
        var res = await httpClient.GetAsync($"{BASE_URL}/api/v4/spot/time");
        var json = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(json);
        return (long)obj.server_time; // Server time đã là milliseconds
    }

    // 2. Sync thời gian với server Gate.io
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
            throw new Exception("Lỗi sync time với Gate.io server", ex);
        }
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + timeOffset;
    }

    // 3. Tạo hash SHA512 từ payload
    private string CreatePayloadHash(string body)
    {
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        using (var sha512 = SHA512.Create())
        {
            var hash = sha512.ComputeHash(bodyBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // 4. Tạo chữ ký HMAC SHA512 cho Gate.io
    private string CreateSignature(string timestamp, string method, string requestPath, string body, string queryString = "")
    {
        var payloadHash = CreatePayloadHash(body);

        var signatureStr = method + "\n" + requestPath + "\n" + queryString + "\n" + payloadHash + "\n" + timestamp;

        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var signatureBytes = Encoding.UTF8.GetBytes(signatureStr);

        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hash = hmac.ComputeHash(signatureBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // 5. Lấy số dư Spot Account
    public async Task<string> GetSpotBalance()
    {
        try
        {
            long timestampMs = GetTimestamp(); // Milliseconds
            string timestampStr = timestampMs.ToString(); // Gate.io API v4 sử dụng milliseconds

            string requestPath = "/api/v4/spot/accounts";
            string method = "GET";
            string body = "";
            string queryString = "";

            string signature = CreateSignature(timestampStr, method, requestPath, body, queryString);

            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_URL}{requestPath}"))
            {
                request.Headers.Add("X-Gate-Access-Key", apiKey);
                request.Headers.Add("X-Gate-Access-Sign", signature);
                request.Headers.Add("Timestamp", timestampStr);

                var res = await httpClient.SendAsync(request);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    throw new Exception($"Gate.io API Error: {res.StatusCode} - {errorContent}");
                }

                return await res.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi lấy số dư từ Gate.io", ex);
        }
    }
}
