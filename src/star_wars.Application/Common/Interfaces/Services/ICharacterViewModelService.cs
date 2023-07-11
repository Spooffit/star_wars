using star_wars.Application.Common.Models.Dto.Character;
using star_wars.Application.Common.Models.ViewModels.Character;

namespace star_wars.Application.Common.Interfaces.Services;

public interface ICharacterViewModelService
{
    public CharacterListViewModel MapCharacterDtoListToViewModel(ICollection<GetCharacterDto> charactersDto);
    public CharacterViewModel MapCharacterDtoToViewModel(GetCharacterDto characterDto);
    
    public UpdateCharacterDto MapViewModelToUpdateCharacterDto(CharacterViewModel characterViewModel);
}