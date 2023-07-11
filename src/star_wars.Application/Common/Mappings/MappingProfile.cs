using AutoMapper;
using star_wars.Application.Common.Models.Dto.Character;
using star_wars.Application.Common.Models.ViewModels.Character;
using star_wars.Core.Entities;

namespace star_wars.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, GetCharacterDto>();

        CreateMap<GetCharacterDto, CharacterViewModel>();
        CreateMap<ICollection<GetCharacterDto>, CharacterListViewModel>()
            .ForMember(dest => dest.Characters, opt =>
                opt.MapFrom(src => src.ToList()));
    }
}