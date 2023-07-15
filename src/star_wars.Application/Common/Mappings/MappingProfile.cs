using AutoMapper;
using star_wars.Application.Common.Models.ViewModels.Character;
using star_wars.Application.Common.Models.ViewModels.Movie;
using star_wars.Core.Entities;

namespace star_wars.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Character, AddCharacterViewModel>().ReverseMap();
        CreateMap<Movie, MovieViewModel>().ReverseMap();
        CreateMap<Character, IndexCharacterViewModel>().ReverseMap();
        CreateMap<Character, InfoCharacterViewModel>().ReverseMap();
        CreateMap<Character, AddCharacterViewModel>().ReverseMap();
        CreateMap<Character, EditCharacterViewModel>().ReverseMap();
    }
}

