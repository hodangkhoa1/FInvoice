using AutoMapper;
using BAL.Models;
using DAL.Entities;

namespace BAL.AutoMapperProfile
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, LoginViewModel>().ReverseMap();
            CreateMap<Account, UserInfoViewModel>();
        }
    }
}
