namespace WebApiApp.Helpers
{
    public static class EnvironmentHelper
    {
        public static string? GetEnv(string key, string? defaultValue = null)
        {
            return Environment.GetEnvironmentVariable(key) ?? defaultValue;
        }
    }

    public static class DbConnectionHelper
    {
        public static string BuildConnectionURI(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            
            return connectionString
                .Replace("${POSTGRES_HOST}", EnvironmentHelper.GetEnv("POSTGRES_HOST"))
                .Replace("${POSTGRES_PORT}", EnvironmentHelper.GetEnv("POSTGRES_PORT"))
                .Replace("${POSTGRES_DB}", EnvironmentHelper.GetEnv("POSTGRES_DB"))
                .Replace("${POSTGRES_USER}", EnvironmentHelper.GetEnv("POSTGRES_USER"))
                .Replace("${POSTGRES_PASSWORD}", EnvironmentHelper.GetEnv("POSTGRES_PASSWORD"));
        }
    }
}
