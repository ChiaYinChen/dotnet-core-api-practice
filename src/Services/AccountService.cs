using AutoMapper;
using WebApiApp.DTOs;
using WebApiApp.Models;
using WebApiApp.Repositories;

namespace WebApiApp.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(AccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<Account> GetAccount(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<Account> CreateAccount(CreateAccountDTO createAccountDto)
        {
            var account = _mapper.Map<Account>(createAccountDto);
            return await _accountRepository.CreateAsync(account);
        }
    }
}