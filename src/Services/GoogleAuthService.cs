using System.Web;
using Microsoft.Extensions.Options;
using WebApiApp.Constants;
using WebApiApp.Helpers;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly HttpRequestService _httpRequestService;

        public GoogleAuthService(
            IOptions<GoogleAuthSettings> googleAuthSettings,
            HttpRequestService httpRequestService
        )
        {
            _googleAuthSettings = googleAuthSettings.Value;
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
            queryParams["client_id"] = _googleAuthSettings.GOOGLE_CLIENT_ID;
            queryParams["redirect_uri"] = _googleAuthSettings.REDIRECT_URL;
            queryParams["scope"] = "openid email profile";
            queryParams["state"] = GenerateRandomState();
            return $"https://accounts.google.com/o/oauth2/auth?{queryParams}";
        }

        public async Task<string> ExchangeCodeForToken(string code)
        {
            var data = new
            {
                code = code,
                client_id = _googleAuthSettings.GOOGLE_CLIENT_ID,
                client_secret = _googleAuthSettings.GOOGLE_CLIENT_SECRET,
                redirect_uri = _googleAuthSettings.REDIRECT_URL,
                grant_type = "authorization_code"
            };
            var (statusCode, responseData) = await _httpRequestService.Post(
                url: "https://oauth2.googleapis.com/token",
                data: data
            );

            if (responseData.TryGetValue("access_token", out var accessToken))
            {
                return accessToken.ToString();
            }
            throw new UnauthorizedError(CustomErrorCode.FailedGetGoogleAccessToken, "Failed to obtain access token");
        }
    }
}
