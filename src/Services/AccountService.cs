using AutoMapper;
using WebApiApp.DTOs;
using WebApiApp.Models;
using WebApiApp.Repositories;
using WebApiApp.Helpers;

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

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _accountRepository.GetAllAsync();
        }
        
        public async Task<Account?> GetAccountByID(Guid id)
        {
            return await _accountRepository.GetByIDAsync(id);
        }
        
        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<Account> CreateAccount(CreateAccountDTO createAccountDto)
        {
            var account = _mapper.Map<Account>(createAccountDto);
            return await _accountRepository.CreateAsync(account);
        }

        public async Task<Account> UpdateAccount(Account accountObj, UpdateAccountDTO updateAccountDto)
        {
            var updatedData = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(updateAccountDto.Password))
            {
                updatedData["HashedPassword"] = SecurityHelper.HashPassword(updateAccountDto.Password);
            }
            if (!string.IsNullOrEmpty(updateAccountDto.Name))
            {
                updatedData["Name"] = updateAccountDto.Name;
            }
            if (updateAccountDto.IsActive.HasValue)
            {
                updatedData["IsActive"] = updateAccountDto.IsActive.Value;
            }
            if (!updatedData.Any())
            {
                throw new ArgumentException("No valid fields to update.");
            }
            
            return await _accountRepository.UpdateAsync(accountObj, updatedData);
        }
    }
}
