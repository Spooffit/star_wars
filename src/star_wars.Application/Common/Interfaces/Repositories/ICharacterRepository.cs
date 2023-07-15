using star_wars.Core.Entities;

namespace star_wars.Application.Common.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<IList<Character>> GetAllCharactersAsync();
    
    Task<Character> GetCharacterByIdAsync(int id);

    Task<Character> AddCharacterAsync(Character newCharacter);

    Task<Character> UpdateCharacterAsync(Character updateCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}