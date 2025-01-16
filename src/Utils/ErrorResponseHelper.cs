using System.Text.Json;
using WebApiApp.Models;

namespace WebApiApp.Helpers
{
    public class ErrorResponseHelper
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task HandleException(HttpContext context, string message, string errorCode, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new Response
            {
                Code = errorCode,
                Message = message,
                Errors = null
            };

            var result = JsonSerializer.Serialize(errorResponse, SerializerOptions);
            await context.Response.WriteAsync(result);
        }
    }
}
