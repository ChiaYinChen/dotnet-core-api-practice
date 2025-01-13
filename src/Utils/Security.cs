namespace WebApiApp.Helpers
{
    public class Security
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
        
}
