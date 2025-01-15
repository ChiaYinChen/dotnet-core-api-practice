using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using WebApiApp.Constants;
using WebApiApp.Models;
using WebApiApp.Helpers;
using WebApiApp.Services;

namespace WebApiApp.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AccountService accountService, JwtHelper jwtHelper)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var jwtToken = jwtHelper.DecodeToken(token);
                    var tokenType = jwtToken.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
                    if (tokenType != "access")
                    {
                        await HandleExceptionAsync(context, "Invalid token type", CustomErrorCode.InvalidTokenType, StatusCodes.Status401Unauthorized);
                    }

                    var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    var account = await accountService.GetAccountByEmail(email: sub);
                    if (account == null)
                    {
                        await HandleExceptionAsync(context, "Account not found", CustomErrorCode.EntityNotFound, StatusCodes.Status404NotFound);
                    }
                    if (!account.IsActive)
                    {
                        await HandleExceptionAsync(context, "Inactive account", CustomErrorCode.InactiveAccount, StatusCodes.Status403Forbidden);
                    }

                    // Account is valid, attach to context
                    context.Items["Account"] = account;
                }

                await _next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                await HandleExceptionAsync(context, "Token expired", CustomErrorCode.TokenExpired, StatusCodes.Status401Unauthorized);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context, "Could not validate credentials", CustomErrorCode.InvalidCredentials, StatusCodes.Status401Unauthorized);
            }
            
        }

        private static Task HandleExceptionAsync(HttpContext context, string message, string errorCode, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new Response
            {
                Code = errorCode,
                Message = message,
                Errors = null
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(errorResponse, options);

            return context.Response.WriteAsync(result);
        }
    }
}
