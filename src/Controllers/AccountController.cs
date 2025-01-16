using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiApp.Authorization;
using WebApiApp.Constants;
using WebApiApp.DTOs;
using WebApiApp.Models;
using WebApiApp.Services;
using WebApiApp.Helpers;

namespace WebApiApp.Controllers
{
    [Authorize]
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

        // GET /api/accounts/me
        [HttpGet("me")]
        public async Task<ActionResult<Response<AccountDTO>>> GetCurrentAccount()
        {
            var account = HttpContext.Items["Account"];
            return Ok(ResponseHelper.Success(
                data: _mapper.Map<AccountDTO>(account)
            ));
        }
        
        // GET /api/accounts
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<AccountDTO>>>> GetAccounts()
        {
            var accounts = await _accountService.GetAllAccounts();
            return Ok(ResponseHelper.Success(
                data: _mapper.Map<List<AccountDTO>>(accounts)
            ));
        }

        // GET /api/accounts/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<AccountDTO>>> GetAccount(Guid id)
        {
            var account = await _accountService.GetAccountByID(id);
            if (account == null)
            {
                throw new NotFoundError(CustomErrorCode.EntityNotFound, "Account not found");
            }
            return Ok(ResponseHelper.Success(
                data: _mapper.Map<AccountDTO>(account)
            ));
        }
        
        // POST: /api/accounts
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Response<AccountDTO>>> CreateAccount([FromBody] CreateAccountDTO createAccountDto)
        {
            var account = await _accountService.GetAccountByEmail(createAccountDto.Email);
            if (account != null)
            {
                throw new ConflictError(CustomErrorCode.EntityConflict, "Email already registered");
            }

            var createdAccount = await _accountService.CreateAccount(createAccountDto);
            var accountDto = _mapper.Map<AccountDTO>(createdAccount);
            return StatusCode(201, ResponseHelper.Success(
                message: "Registered successfully",
                data: accountDto
            ));
        }

        // PATCH: /api/accounts/:id
        [HttpPatch("{id}")]
        public async Task<ActionResult<Response<AccountDTO>>> UpdateAccount([FromRoute] Guid id, [FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var account = await _accountService.GetAccountByID(id);
            if (account == null)
            {
                throw new NotFoundError(CustomErrorCode.EntityNotFound, "Account not found");
            }

            var updatedAccount = await _accountService.UpdateAccount(account, updateAccountDTO);
            return Ok(ResponseHelper.Success(
                message: "Updated Successfully",
                data: _mapper.Map<AccountDTO>(updatedAccount)
            ));
        }

        // DELETE: /api/accounts/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            var account = await _accountService.GetAccountByID(id);
            if (account == null)
            {
                throw new NotFoundError(CustomErrorCode.EntityNotFound, "Account not found");
            }
            
            await _accountService.DeleteAccount(account);
            return NoContent();
        }
    }
}
