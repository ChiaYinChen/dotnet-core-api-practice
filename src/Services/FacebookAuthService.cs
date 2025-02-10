using System.Web;
using Microsoft.Extensions.Options;
using WebApiApp.Constants;
using WebApiApp.Helpers;
using WebApiApp.Models;

namespace WebApiApp.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly FacebookAuthSettings _facebookAuthSettings;
        private readonly HttpRequestService _httpRequestService;

        public FacebookAuthService(
            IOptions<FacebookAuthSettings> facebookAuthSettings,
            HttpRequestService httpRequestService
        )
        {
            _facebookAuthSettings = facebookAuthSettings.Value;
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
            queryParams["client_id"] = _facebookAuthSettings.FACEBOOK_CLIENT_ID;
            queryParams["redirect_uri"] = _facebookAuthSettings.FACEBOOK_REDIRECT_URL;
            queryParams["scope"] = "email public_profile";
            queryParams["state"] = GenerateRandomState();
            return $"https://www.facebook.com/v22.0/dialog/oauth?{queryParams}";
        }

        public async Task<string> ExchangeCodeForToken(string code)
        {
            var data = new
            {
                code = code,
                client_id = _facebookAuthSettings.FACEBOOK_CLIENT_ID,
                client_secret = _facebookAuthSettings.FACEBOOK_CLIENT_SECRET,
                redirect_uri = _facebookAuthSettings.FACEBOOK_REDIRECT_URL
            };
            var (_, responseData) = await _httpRequestService.Post(
                url: "https://graph.facebook.com/v22.0/oauth/access_token",
                data: data
            );

            if (responseData.TryGetValue("access_token", out var accessToken))
            {
                return accessToken.ToString()!;
            }
            throw new UnauthorizedError(CustomErrorCode.FailedGetFacebookAccessToken, "Failed to obtain access token");
        }

        public async Task<Dictionary<string, object>> GetUserInfo(string accessToken)
        {
            var (statusCode, responseData) = await _httpRequestService.Get(
                url: "https://graph.facebook.com/v22.0/me",
                queryParams: new Dictionary<string, string> {
                    { "fields", "id,name,email,picture" },
                    { "access_token", accessToken}
                }
            );
            return responseData;
        }
    }
}
