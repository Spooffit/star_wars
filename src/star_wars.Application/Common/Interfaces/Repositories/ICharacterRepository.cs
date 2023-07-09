using star_wars.Core.Entities;

namespace star_wars.Application.Common.Interfaces.Repositories;

public interface ICharacterRepository
{
    Task<ICollection<Character>> GetAllCharactersAsync();
    
    Task<Character> GetCharacterByIdAsync(int id);
    
    Task AddCharacterAsync(Character newCharacter);

    Task UpdateCharacterAsync(Character updateCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}