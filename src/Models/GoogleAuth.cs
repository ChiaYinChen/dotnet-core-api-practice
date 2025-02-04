namespace WebApiApp.Models
{
    public class GoogleAuthSettings
    {
        public required string GOOGLE_CLIENT_ID { get; set; }
        public required string GOOGLE_CLIENT_SECRET { get; set; }
        public required string REDIRECT_URL { get; set; }
    }

    public class GoogleAuthUrl
    {
        public required string authorization_url { get; set; }
    }
}
