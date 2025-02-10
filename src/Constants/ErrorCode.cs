namespace WebApiApp.Constants
{
    public static class CustomErrorCode
    {
        // Entity errors
        public const string EntityConflict = "1001";                 // Conflict between entities in the system
        public const string EntityNotFound = "1002";                 // Entity not found

        // User-related errors
        public const string ResetPasswordMismatch = "2101";          // Mismatch between reset password request and user

        // Auth errors
        public const string NotAuthenticated = "4001";               // User is not authenticated
        public const string InvalidCredentials = "4002";             // Provided credentials are invalid
        public const string TokenExpired = "4003";                   // Authentication token has expired
        public const string InvalidTokenType = "4004";               // Invalid type of authentication token
        public const string TokenRevoked = "4005";                   // Authentication token has been revoked
        public const string InactiveAccount = "4006";                // Account is inactive
        public const string OperationNotPermitted = "4007";          // Operation is not permitted for the user

        // Login errors
        public const string IncorrectEmailPassword = "4101";         // Incorrect email or password
        public const string InvalidStateParameter = "4102";          // Invalid state parameter in login attempt
        public const string FailedGetGoogleAccessToken = "4103";     // Failed to obtain Google access token
        public const string FailedGetFacebookAccessToken = "4104";   // Failed to obtain Facebook access token

        // General errors
        public const string ValidateError = "9100";                  // General validation error
        public const string InvalidOperation = "9200";               // Invalid operation
    }
}
