namespace WebApiApp.Models
{
    public class GoogleAuthSettings
    {
        public required string GOOGLE_CLIENT_ID { get; set; }
        public required string GOOGLE_CLIENT_SECRET { get; set; }
        public required string GOOGLE_REDIRECT_URL { get; set; }
    }

    public class FacebookAuthSettings
    {
        public required string FACEBOOK_CLIENT_ID { get; set; }
        public required string FACEBOOK_CLIENT_SECRET { get; set; }
        public required string FACEBOOK_REDIRECT_URL { get; set; }
    }

    public class AuthUrl
    {
        public required string authorization_url { get; set; }
    }
}
