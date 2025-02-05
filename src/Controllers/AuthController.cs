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
        private readonly GoogleAuthService _googleAuthService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(
            AccountService accountService,
            GoogleAuthService googleAuthService,
            JwtHelper jwtHelper
        )
        {
            _accountService = accountService;
            _googleAuthService = googleAuthService;
            _jwtHelper = jwtHelper;
        }

        // POST: /api/auth/login
        [HttpPost("login")]
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

        // GET: /api/auth/:provider/authorize
        [HttpGet("{provider}/authorize")]
        public ActionResult<AuthUrl> Authorize([FromRoute] string provider)
        {
            var providerServices = new Dictionary<string, IAuthService>
            {
                { "google", _googleAuthService }
            };
            if (!providerServices.TryGetValue(provider.ToLower(), out var authService))
            {
                throw new BadRequestError(CustomErrorCode.ValidateError, "Invalid provider");
            }
            return Ok(new AuthUrl{ authorization_url = authService.BuildAuthUrl() });
        }

        // GET: /api/auth/:provider/callback
        [HttpGet("{provider}/callback")]
        public async Task<IActionResult> Callback([FromRoute] string provider, [FromQuery] AuthCallbackRequest request)
        {
            var providerServices = new Dictionary<string, IAuthService>
            {
                { "google", _googleAuthService }
            };
            if (!providerServices.TryGetValue(provider.ToLower(), out var authService))
            {
                throw new BadRequestError(CustomErrorCode.ValidateError, "Invalid provider");
            }
            if (request.state != null && !authService.ValidateState(request.state))
            {
                throw new BadRequestError(CustomErrorCode.InvalidStateParameter, "Invalid state");
            }

            var accessToken = await authService.ExchangeCodeForToken(request.code);
            return Ok(new { data = accessToken });
        }
    }
}
