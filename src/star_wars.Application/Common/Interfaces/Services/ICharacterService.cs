using star_wars.Application.Common.Models.ViewModels.Character;

namespace star_wars.Application.Common.Interfaces.Services;

public interface ICharacterService
{
    Task<IndexCharacterListViewModel> GetAllIndexCharactersAsync();
    
    Task<InfoCharacterViewModel> GetInfoCharacterByIdAsync(int id);
    
    Task<EditCharacterViewModel> GetEditCharacterByIdAsync(int id);
    Task<AddCharacterViewModel> GetAddCharacterAsync();

    Task<AddCharacterViewModel> AddCharacterAsync(AddCharacterViewModel newAddCharacter);
    
    Task<EditCharacterViewModel> UpdateCharacterAsync(EditCharacterViewModel updateAddCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}