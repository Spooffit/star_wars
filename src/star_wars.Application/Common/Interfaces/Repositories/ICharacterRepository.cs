using star_wars.Core.Entities;
using X.PagedList;

namespace star_wars.Application.Common.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IPagedList<Character>> GetPagedCharactersAsync(int page, int pageSize);
    
    Task<int> GetTotalCharacterCountAsync();
    
    Task<Character> GetCharacterByIdAsync(int id);

    Task<Character> AddCharacterAsync(Character newCharacter);

    Task<Character> UpdateCharacterAsync(Character updateCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}