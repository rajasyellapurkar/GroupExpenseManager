using AutoMapper;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Helper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForCreationDto,User>();
            CreateMap<User,UserToReturnDto>()
                        .ForMember(u=>u.Age, opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            CreateMap<GroupForCreationDto,Group>();
            CreateMap<Group,GroupToReturnDto>();
            CreateMap<Account,AccountForCreationDto>().ReverseMap();
        }
    }
}