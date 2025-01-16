using WebApiApp.Helpers;

namespace WebApiApp.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exc)
            {
                // Set default error response details for unexpected exceptions
                var statusCode = StatusCodes.Status500InternalServerError;
                var errorCode = "9999";
                var message = exc.Message;

                // If error is CustomError, customize the response
                if (exc is CustomError customError)
                {
                    statusCode = customError.StatusCode;
                    errorCode = customError.ErrorCode;
                    message = customError.Message;
                }

                await ErrorResponseHelper.HandleException(context, message, errorCode, statusCode);
            }
        }
    }
}
