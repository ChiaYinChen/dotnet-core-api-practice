namespace WebApiApp.Helpers
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
        
}
