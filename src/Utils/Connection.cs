namespace WebApiApp.Helpers
{
    public class DbConnectionHelper
    {
        public static string BuildPostgresConnection(string connectionString)
        {           
            return connectionString
                .Replace("${POSTGRES_HOST}", EnvironmentHelper.GetEnv("POSTGRES_HOST"))
                .Replace("${POSTGRES_PORT}", EnvironmentHelper.GetEnv("POSTGRES_PORT"))
                .Replace("${POSTGRES_DB}", EnvironmentHelper.GetEnv("POSTGRES_DB"))
                .Replace("${POSTGRES_USER}", EnvironmentHelper.GetEnv("POSTGRES_USER"))
                .Replace("${POSTGRES_PASSWORD}", EnvironmentHelper.GetEnv("POSTGRES_PASSWORD"));
        }
    }
}
