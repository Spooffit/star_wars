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
        CreateMap<AddCharacterDto, Character>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, GetCharacterDto>();
        
        CreateMap<ICollection<GetCharacterDto>, CharacterListViewModel>();
        CreateMap<GetCharacterDto, CharacterViewModel>();
    }
}