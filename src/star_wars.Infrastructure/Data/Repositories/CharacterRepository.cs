using Microsoft.EntityFrameworkCore;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<Character> _dbSet;

    public CharacterRepository(
        ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<Character>();
    }
    public async Task<IList<Character>> GetAllCharactersAsync()
    {
        return await _dbSet
            .Include(c => c.Movies)
            .Include(c => c.Planet)
            .ToListAsync();
    }

    public async Task<Character> GetCharacterByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Movies)
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Character> AddCharacterAsync(Character newCharacter)
    {
        await _dbSet.AddAsync(newCharacter);
        await SaveChangesAsync();
        return newCharacter;
    }

    public async Task<Character> UpdateCharacterAsync(Character updateCharacter)
    {
        _dbSet.Update(updateCharacter);
        await SaveChangesAsync();
        return updateCharacter;
    }

    public async Task<bool> DeleteCharacterByIdAsync(int id)
    {
        var character = await _dbSet.FindAsync(id);
        if (character == null)
            return false;
        
        _dbSet.Remove(character);
        await SaveChangesAsync();
        
        return true;
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}