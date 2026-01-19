using AutoMapper;
using Domain.Entities;
using Application.Dtos.Users;

namespace Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
