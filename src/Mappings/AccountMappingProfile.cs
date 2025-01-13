using AutoMapper;
using WebApiApp.DTOs;
using WebApiApp.Models;
using WebApiApp.Helpers;

namespace WebApiApp.MappingProfiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            // Map CreateAccountDTO to Account (for adding new account)
            // Source: CreateAccountDTO and Destination: Account
            CreateMap<CreateAccountDTO, Account>()
                .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => SecurityHelper.HashPassword(src.Password)));

            // Map Account to AccountDTO
            // Source: Account and Destination: AccountDTO
            CreateMap<Account, AccountDTO>();
        }
    }
}
