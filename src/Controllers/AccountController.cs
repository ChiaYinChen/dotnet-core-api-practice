using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiApp.DTOs;
using WebApiApp.Services;
using WebApiApp.Helpers;

namespace WebApiApp.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(AccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        // POST: /api/accounts
        [HttpPost]
        public async Task<ActionResult<Response<AccountDTO>>> CreateAccount([FromBody] CreateAccountDTO createAccountDto)
        {
            var account = await _accountService.GetAccount(createAccountDto.Email);
            if (account != null)
            {
                return StatusCode(409, ResponseHelper.Error(
                    message: "Email already registered"
                ));
            }

            var createdAccount = await _accountService.CreateAccount(createAccountDto);
            var accountDto = _mapper.Map<AccountDTO>(createdAccount);
            return StatusCode(201, ResponseHelper.Success(
                message: "Registered successfully",
                data: accountDto
            ));
        }
    }
}
