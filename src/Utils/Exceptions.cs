namespace WebApiApp.Helpers
{
    public class CustomError : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        public CustomError(int statusCode, string errorCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    public class UnauthenticatedError : CustomError
    {
        public UnauthenticatedError(string errorCode, string message)
            : base(StatusCodes.Status401Unauthorized, errorCode, message) { }
    }

    public class UnauthorizedError : CustomError
    {
        public UnauthorizedError(string errorCode, string message)
            : base(StatusCodes.Status403Forbidden, errorCode, message) { }
    }

    public class ConflictError : CustomError
    {
        public ConflictError(string errorCode, string message)
            : base(StatusCodes.Status409Conflict, errorCode, message) { }
    }

    public class NotFoundError : CustomError
    {
        public NotFoundError(string errorCode, string message)
            : base(StatusCodes.Status404NotFound, errorCode, message) { }
    }

    public class BadRequestError : CustomError
    {
        public BadRequestError(string errorCode, string message)
            : base(StatusCodes.Status400BadRequest, errorCode, message) { }
    }
}
