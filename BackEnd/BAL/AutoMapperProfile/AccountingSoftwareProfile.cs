using AutoMapper;
using BAL.Models;
using DAL.Entities;

namespace BAL.AutoMapperProfile
{
    public class AccountingSoftwareProfile : Profile
    {
        public AccountingSoftwareProfile()
        {
            CreateMap<AccountingSoftware, AccountingSoftwareInfoViewModel>();
        }
    }
}
