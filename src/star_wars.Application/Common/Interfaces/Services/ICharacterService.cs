using star_wars.Application.Common.Models;

namespace star_wars.Application.Common.Interfaces.Services;

public interface ICharacterService
{
    Task<ICollection<GetCharacterDto>> GetAllCharactersAsync();
    
    Task<GetCharacterDto> GetCharacterByIdAsync(int id);
    
    Task<GetCharacterDto> AddCharacterAsync(AddCharacterDto newCharacter);
    
    Task<GetCharacterDto> UpdateCharacterAsync(UpdateCharacterDto updateCharacter);
    
    Task<bool> DeleteCharacterByIdAsync(int id);
}