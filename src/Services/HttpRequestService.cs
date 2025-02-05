using System.Net;
using System.Text;
using System.Text.Json;

namespace WebApiApp.Services
{
    public class HttpRequestService
    {
        private readonly HttpClient _httpClient;

        public HttpRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(HttpStatusCode StatusCode, Dictionary<string, object> JsonData)> Get(string url)
        {
            try
            {
                using var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var jsonData = TryParseJson(content);
                return (response.StatusCode, jsonData);
            }
            catch (HttpRequestException exc)
            {
                return HandleHttpException(exc);
            }
        }

        public async Task<(HttpStatusCode StatusCode, Dictionary<string, object> JsonData)> Post<T>(string url, T data)
        {
            try
            {
                var json = data is not null ? JsonSerializer.Serialize(data) : "{}";
                using var body = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync(url, body);
                var content = await response.Content.ReadAsStringAsync();

                var jsonData = TryParseJson(content);
                return (response.StatusCode, jsonData);
            }
            catch (HttpRequestException exc)
            {
                return HandleHttpException(exc);
            }
        }

        private static Dictionary<string, object> TryParseJson(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new Dictionary<string, object>();
            }
            catch (JsonException)
            {
                return new Dictionary<string, object> { { "error", "Invalid JSON response" } };
            }
        }

        private static (HttpStatusCode, Dictionary<string, object>) HandleHttpException(HttpRequestException exc)
        {
            return (HttpStatusCode.ServiceUnavailable, new Dictionary<string, object> { { "error", exc.Message } });
        }
    }
}
