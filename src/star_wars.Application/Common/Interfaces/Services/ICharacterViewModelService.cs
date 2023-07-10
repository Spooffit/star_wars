using star_wars.Application.Common.Models.Dto.Character;
using star_wars.Application.Common.Models.ViewModels.Character;

namespace star_wars.Application.Common.Interfaces.Services;

public interface ICharacterViewModelService
{
    public CharacterListViewModel MapCharacterListToViewModel(ICollection<GetCharacterDto> charactersDto);
    public CharacterViewModel MapCharacterToViewModel(GetCharacterDto charactersDto);
}