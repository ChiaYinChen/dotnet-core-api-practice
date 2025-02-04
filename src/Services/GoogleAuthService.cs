using System.Web;
using Microsoft.Extensions.Options;
using WebApiApp.Models;


namespace WebApiApp.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthService(IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings.Value;
        }

        public string GenerateRandomState()
        {
            return Guid.NewGuid().ToString("N");  // 32-character hexadecimal string
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
    }
}
