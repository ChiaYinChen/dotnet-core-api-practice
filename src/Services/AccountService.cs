using AutoMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApiApp.Constants;
using WebApiApp.DTOs;
using WebApiApp.Entities;
using WebApiApp.Models;
using WebApiApp.Repositories;
using WebApiApp.Helpers;

namespace WebApiApp.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly SocialAccountRepository _socialAccountRepository;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(
            AccountRepository accountRepository,
            SocialAccountRepository socialAccountRepository,
            EmailService emailService,
            IMapper mapper
        )
        {
            _accountRepository = accountRepository;
            _socialAccountRepository = socialAccountRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<(List<Account> Data, Pagination Paging)> GetAllAccounts(int pageNumber, int pageSize, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "Id";  // default
            }
            Expression<Func<Account, object>> orderByExpression = a => EF.Property<object>(a, orderBy);
            
            var query = await _accountRepository.GetAllAsync(orderByExpression);
            return await PaginationHelper.ApplyPagination(query, pageNumber, pageSize);
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
            var createdAccount = await _accountRepository.CreateAsync(account);
            await _emailService.SendRegisterSuccessEmail(createdAccount.Email, createdAccount.Name!);
            return createdAccount;
        }

        public async Task<Account> CreateAccountWithSocial(string provider, Dictionary<string, object> socialInfo)
        {
            var email = socialInfo["email"].ToString();
            var name = socialInfo.GetValueOrDefault("name")?.ToString();
            var uniqueId = socialInfo.GetValueOrDefault("id")?.ToString() ?? socialInfo.GetValueOrDefault("sub")?.ToString();

            var account = await _accountRepository.GetByEmailAsync(email!);
            bool isNewAccount = account == null;
            
            if (isNewAccount)
            {
                // Create account
                var createAccountDto = new CreateAccountDTO
                {
                    Email = email!,
                    Name = name,
                    Password = Guid.NewGuid().ToString("N")  // Random password
                };
                account = _mapper.Map<Account>(createAccountDto);
                account = await _accountRepository.CreateAsync(account);
            }

            var socialAccount = await _socialAccountRepository.GetByProviderAsync(provider, uniqueId!);
            if (socialAccount == null)
            {
                // Create social account
                var createSocialAccountDto = new CreateSocialAccountDTO
                {
                    Provider = provider,
                    UniqueId = uniqueId!,
                    AccountId = account!.Id
                };
                socialAccount = _mapper.Map<SocialAccount>(createSocialAccountDto);
                await _socialAccountRepository.CreateAsync(socialAccount);
            }

            if (isNewAccount)
            {
                await _emailService.SendRegisterSuccessEmail(account!.Email, account.Name!);
            }
            
            return account!;
        }

        public async Task<Account> UpdateAccount(Account accountObj, UpdateAccountDTO updateAccountDto)
        {
            var updatedData = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(updateAccountDto.Password))
            {
                updatedData["HashedPassword"] = Security.HashPassword(updateAccountDto.Password);
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
                throw new BadRequestError(CustomErrorCode.ValidateError, "No valid fields to update");
            }
            
            return await _accountRepository.UpdateAsync(accountObj, updatedData);
        }

        public async Task<Account> DeleteAccount(Account accountObj)
        {
            return await _accountRepository.RemoveAsync(accountObj);
        }

        public async Task<Account?> Authenticate(string email, string password)
        {
            var account = await GetAccountByEmail(email);
            if (account == null)
            {
                return null;
            }
            if (!Security.VerifyPassword(password, account.HashedPassword!))
            {
                return null;
            }
            return account;
        }
    }
}
