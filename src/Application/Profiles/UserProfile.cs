using AutoMapper;
using api_catalogo.Data.Dtos;
using api_catalogo.Models;

namespace api_catalogo.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
