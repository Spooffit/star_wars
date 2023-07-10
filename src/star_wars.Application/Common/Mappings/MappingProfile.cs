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

        CreateMap<ICollection<GetCharacterDto>, CharacterListViewModel>()
            .ForMember(dest => dest.Characters, opt =>
                opt.MapFrom(src =>
                    src.Select(dto => new CharacterViewModel
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        OriginName = dto.OriginName,
                        Birthdate = dto.Birthdate,
                        Planet = dto.Planet,
                        Gender = dto.Gender,
                        Species = dto.Species,
                        Height = dto.Height,
                        HairColor = dto.HairColor,
                        EyeColor = dto.EyeColor,
                        Description = dto.Description,
                        Movies = dto.Movies.ToList()
                    }).ToList()));

        CreateMap<GetCharacterDto, CharacterViewModel>();
    }
}