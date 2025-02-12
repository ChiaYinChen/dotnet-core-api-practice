using System.Web;
using Microsoft.Extensions.Options;
using WebApiApp.Constants;
using WebApiApp.Helpers;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class LineAuthService : IAuthService
    {
        private readonly LineAuthSettings _lineAuthSettings;
        private readonly HttpRequestService _httpRequestService;

        public LineAuthService(
            IOptions<LineAuthSettings> lineAuthSettings,
            HttpRequestService httpRequestService
        )
        {
            _lineAuthSettings = lineAuthSettings.Value;
            _httpRequestService = httpRequestService;
        }

        public string GenerateRandomState()
        {
            return Guid.NewGuid().ToString("N");  // 32-character hexadecimal string
        }

        public bool ValidateState(string state)
        {
            // TODO: implement the validation logic for the state parameter
            return true;
        }

        public string BuildAuthUrl()
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["response_type"] = "code";
            queryParams["client_id"] = _lineAuthSettings.LINE_CLIENT_ID;
            queryParams["redirect_uri"] = _lineAuthSettings.LINE_REDIRECT_URL;
            queryParams["scope"] = "profile openid email";
            queryParams["state"] = GenerateRandomState();
            return $"https://access.line.me/oauth2/v2.1/authorize?{queryParams}";
        }

        public async Task<string> ExchangeCodeForToken(string code)
        {
            var data = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _lineAuthSettings.LINE_CLIENT_ID },
                { "client_secret", _lineAuthSettings.LINE_CLIENT_SECRET },
                { "redirect_uri", _lineAuthSettings.LINE_REDIRECT_URL },
                { "grant_type", "authorization_code" }
            };
            var (_, responseData) = await _httpRequestService.Post(
                url: "https://api.line.me/oauth2/v2.1/token",
                headers: new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } },
                data: data
            );

            if (responseData.TryGetValue("id_token", out var accessToken))
            {
                return accessToken.ToString()!;
            }
            throw new UnauthorizedError(CustomErrorCode.FailedGetLineAccessToken, "Failed to obtain access token");
        }

        public async Task<Dictionary<string, object>> GetUserInfo(string accessToken)
        {
            var data = new Dictionary<string, string>
            {
                { "id_token", accessToken },
                { "client_id", _lineAuthSettings.LINE_CLIENT_ID }
            };
            var (statusCode, responseData) = await _httpRequestService.Post(
                url: "https://api.line.me/oauth2/v2.1/verify",
                headers: new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } },
                data: data
            );
            return responseData;
        }
    }
}
