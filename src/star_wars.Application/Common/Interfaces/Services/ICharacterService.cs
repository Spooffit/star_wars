using star_wars.Application.Common.Models.ViewModels.Character;
using X.PagedList;

namespace star_wars.Application.Common.Interfaces.Services;

public interface ICharacterService
{
    Task<IPagedList<IndexCharacterViewModel>> GetPagedIndexCharactersAsync(int page, int pageSize);

    Task<IPagedList<IndexCharacterViewModel>> GetPagedIndexCharactersAsync(        
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender, 
        int page, 
        int pageSize);

    Task<bool> IsCharacterOwnerAsync(int characterId, Guid userId);
    Task<EditCharacterViewModel> GetEditCharacterByIdAsync(int id);
    Task<InfoCharacterViewModel> GetInfoCharacterByIdAsync(int id);

    Task<AddCharacterViewModel> GetAddCharacterAsync();

    Task<AddCharacterViewModel> AddCharacterAsync(AddCharacterViewModel newAddCharacter);

    Task<EditCharacterViewModel> UpdateCharacterAsync(EditCharacterViewModel updateAddCharacter);

    Task<bool> DeleteCharacterByIdAsync(int id);
}