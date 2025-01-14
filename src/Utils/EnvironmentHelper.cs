namespace WebApiApp.Helpers
{
    public class EnvironmentHelper
    {
        public static string? GetEnv(string key, string? defaultValue = null)
        {
            return Environment.GetEnvironmentVariable(key) ?? defaultValue;
        }
    }
}
