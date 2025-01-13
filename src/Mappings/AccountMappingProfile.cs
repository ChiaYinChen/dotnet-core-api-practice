using AutoMapper;
using WebApiApp.DTOs;
using WebApiApp.Entities;
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
                .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => Security.HashPassword(src.Password)));

            // Map Account to AccountDTO
            // Source: Account and Destination: AccountDTO
            CreateMap<Account, AccountDTO>();
        }
    }
}
