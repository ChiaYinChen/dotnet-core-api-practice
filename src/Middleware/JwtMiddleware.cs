using Microsoft.IdentityModel.Tokens;
using WebApiApp.Constants;
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
                        await ErrorResponseHelper.HandleException(context, "Invalid token type", CustomErrorCode.InvalidTokenType, StatusCodes.Status401Unauthorized);
                        return;
                    }

                    var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    var account = await accountService.GetAccountByEmail(email: sub!);
                    if (account == null)
                    {
                        await ErrorResponseHelper.HandleException(context, "Account not found", CustomErrorCode.EntityNotFound, StatusCodes.Status404NotFound);
                        return;
                    }
                    if (!account.IsActive)
                    {
                        await ErrorResponseHelper.HandleException(context, "Inactive account", CustomErrorCode.InactiveAccount, StatusCodes.Status403Forbidden);
                        return;
                    }

                    // Account is valid, attach to context
                    context.Items["Account"] = account;
                }

                await _next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                await ErrorResponseHelper.HandleException(context, "Token expired", CustomErrorCode.TokenExpired, StatusCodes.Status401Unauthorized);
                return;
            }
            catch (Exception)
            {
                await ErrorResponseHelper.HandleException(context, "Could not validate credentials", CustomErrorCode.InvalidCredentials, StatusCodes.Status401Unauthorized);
                return;
            }
            
        }
    }
}
