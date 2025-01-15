using Microsoft.AspNetCore.Mvc;
using WebApiApp.Constants;
using WebApiApp.Models;
using WebApiApp.Services;
using WebApiApp.Helpers;

namespace WebApiApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(AccountService accountService, JwtHelper jwtHelper)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
        }

        // POST: /api/auth/access-token
        [HttpPost("access-token")]
        public async Task<ActionResult<Token>> Login([FromBody] LoginRequest request)
        {
            var account = await _accountService.Authenticate(request.Email, request.Password);
            if (account == null)
            {
                throw new UnauthenticatedError(CustomErrorCode.IncorrectEmailPassword, "Incorrect email or password");
            }
            if (!account.IsActive)
            {
                throw new UnauthorizedError(CustomErrorCode.InactiveAccount, "Inactive account");
            }
            return Ok(new Token
            {
                access_token = _jwtHelper.CreateAccessToken(sub: account.Email),
                refresh_token = _jwtHelper.CreateRefreshToken(sub: account.Email),
                token_type = "bearer"
            });
        }
    }
}
