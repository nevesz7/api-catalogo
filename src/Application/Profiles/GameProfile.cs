using AutoMapper;
using Domain.Entities;
using Application.Dtos.Games;

namespace Application.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GetGameDto>().ReverseMap();
            CreateMap<CreateGameDto, Game>();
            CreateMap<UpdateGameDto, Game>();
        }
    }
}
