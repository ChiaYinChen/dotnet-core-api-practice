namespace WebApiApp.Services
{
    public interface IAuthService
    {
        string BuildAuthUrl();
        bool ValidateState(string state);
        Task<string> ExchangeCodeForToken(string code);
        Task<Dictionary<string, object>> GetUserInfo(string accessToken);
    }
}
