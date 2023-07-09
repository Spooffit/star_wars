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
    public async Task<ICollection<Character>> GetAllCharactersAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Character> GetCharacterByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddCharacterAsync(Character newCharacter)
    {
        await _dbSet.AddAsync(newCharacter);
        await SaveChangesAsync();
    }

    public async Task UpdateCharacterAsync(Character updateCharacter)
    {
        _dbSet.Update(updateCharacter);
        await SaveChangesAsync();
    }

    public async Task<bool> DeleteCharacterByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
            return true;
        }

        return false;
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}