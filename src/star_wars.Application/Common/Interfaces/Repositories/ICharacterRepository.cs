using star_wars.Core.Entities;
using X.PagedList;

namespace star_wars.Application.Common.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IPagedList<Character>> GetPagedCharactersAsync(int page, int pageSize);

    Task<IPagedList<Character>> GetPagedCharactersAsync(        
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender, 
        int page, 
        int pageSize);
    
    Task<int> GetTotalCharacterCountAsync();

    Task<int> GetFilteredCharacterCountAsync(        
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender);
    
    Task<Character> GetCharacterByIdAsync(int id);

    Task<Character> AddCharacterAsync(Character newCharacter);

    Task<Character> UpdateCharacterAsync(Character updateCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}