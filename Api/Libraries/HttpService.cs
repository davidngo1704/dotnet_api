using System.Text.Json;

namespace Api.Libraries
{
    public interface IHttpService
    {

        Task<T?> GetAsync<T>(string url);
        Task<TResponse?> PostAsync<TRequest, TResponse>(
            string url,
            TRequest body,
            CancellationToken ct = default);
    }
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                using var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    throw new HttpRequestException(
                        $"Request failed with status {response.StatusCode}",
                        null,
                        response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<T>(DefaultValue.JsonOption);
                return result;
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Request timeout: {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during GET request to {url}: {ex.Message}");
                return default;
            }
        }

        // ================= POST =================
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string url,
            TRequest body,
            CancellationToken ct = default)
        {
            try
            {
                using var response = await _httpClient.PostAsJsonAsync(url, body, DefaultValue.JsonOption, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(ct);

                    throw new HttpRequestException(
                        $"Request failed with status {response.StatusCode}",
                        null,
                        response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<TResponse>(DefaultValue.JsonOption, ct);
                return result;
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Request timeout: {url}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
