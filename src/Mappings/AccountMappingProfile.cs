using AutoMapper;
using WebApiApp.DTOs;
using WebApiApp.Models;

namespace WebApiApp.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            // Map CreateAccountDTO to Account (for adding new account)
            // Source: CreateAccountDTO and Destination: Account
            CreateMap<CreateAccountDTO, Account>()
                .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => HashPassword(src.Password)));

            // Map Account to AccountDTO
            // Source: Account and Destination: AccountDTO
            CreateMap<Account, AccountDTO>();
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
