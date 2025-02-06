using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace WebApiApp.Services
{
    public class HttpRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpRequestService> _logger;

        public HttpRequestService(HttpClient httpClient, ILogger<HttpRequestService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(HttpStatusCode StatusCode, Dictionary<string, object> JsonData)> Get(
            string url, 
            Dictionary<string, string>? queryParams = null, 
            Dictionary<string, string>? headers = null
        )
        {
            try
            {
                // Query Parameters
                if (queryParams?.Count > 0)
                {
                    var queryString = string.Join("&", queryParams.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
                    url = $"{url}?{queryString}";
                }

                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Headers
                if (headers?.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                // Send Request
                using var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                var jsonData = TryParseJson(content);
                return (response.StatusCode, jsonData);
            }
            catch (HttpRequestException exc)
            {
                return HandleHttpException(exc);
            }
        }

        public async Task<(HttpStatusCode StatusCode, Dictionary<string, object> JsonData)> Post<T>(
            string url,
            T? data,
            Dictionary<string, string>? headers = null
        )
        {
            try
            {
                // Request Data
                var json = data is not null ? JsonSerializer.Serialize(data) : "{}";
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                // Headers
                if (headers?.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                // Send Request
                using var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                var jsonData = TryParseJson(content);
                return (response.StatusCode, jsonData);
            }
            catch (Exception exc)
            {
                return HandleHttpException(exc);
            }
        }

        private Dictionary<string, object> TryParseJson(string content)
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

        private (HttpStatusCode, Dictionary<string, object>) HandleHttpException(Exception exc)
        {
            var statusCode = (exc as HttpRequestException)?.StatusCode ?? HttpStatusCode.ServiceUnavailable;
            _logger.LogError(exc, "[HttpRequestService] Error: {ErrorMessage}, StatusCode: {StatusCode}", exc.Message, statusCode);
            return (statusCode, new Dictionary<string, object> { { "error", exc.Message } });
        }
    }
}
