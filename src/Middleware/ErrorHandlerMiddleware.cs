using System.Net;
using System.Text.Json;
using WebApiApp.Models;
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                // Set the default HTTP status code to 500 (Internal Server Error)
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Create a generic error response object
                var errorResponse = new Response
                {
                    Code = "9999",
                    Message = error.Message,
                    Errors = null
                };

                // If the error is of type CustomError, customize the response
                if (error is CustomError customError)
                {
                    response.StatusCode = customError.StatusCode;
                    errorResponse = new Response
                    {
                        Code = customError.ErrorCode,
                        Message = customError.Message,
                        Errors = null
                    };
                }

                // Set JSON serializer options to use camelCase for property names
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                
                // Serialize the error response object into JSON format
                var result = JsonSerializer.Serialize(errorResponse, options);
                await response.WriteAsync(result);
            }
        }
    }
}
