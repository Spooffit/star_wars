using Microsoft.EntityFrameworkCore;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Core.Entities;
using X.PagedList;

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

    public async Task<IPagedList<Character>> GetPagedCharactersAsync(int page, int pageSize)
    {
        return await _dbSet.ToPagedListAsync(page, pageSize);
    }

    public async Task<IPagedList<Character>> GetPagedCharactersAsync(        
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender, 
        int page, 
        int pageSize)
    {
        var query = _dbSet.AsQueryable();

        if (searchBirthDateFrom.HasValue)
        {
            query = query.Where(c => c.Birthdate >= searchBirthDateFrom);
        }

        if (searchBirthDateTo.HasValue)
        {
            query = query.Where(c => c.Birthdate <= searchBirthDateTo);
        }

        if (!string.IsNullOrEmpty(searchPlanet))
        {
            query = query.Where(c => c.Planet == searchPlanet);
        }

        if (!string.IsNullOrEmpty(searchGender))
        {
            query = query.Where(c => c.Gender == searchGender);
        }

        if (searchMovies != null)
        {
            query = query
                .Where(c => c.Movies.Any(m => searchMovies.Contains(m.Title)));
        }

        return await query.ToPagedListAsync(page, pageSize);
    }

    public async Task<int> GetFilteredCharacterCountAsync(        
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender)
    {
        var query = _dbSet.AsQueryable();
        
        if (searchBirthDateFrom.HasValue)
        {
            query = query.Where(c => c.Birthdate >= searchBirthDateFrom);
        }

        if (searchBirthDateTo.HasValue)
        {
            query = query.Where(c => c.Birthdate <= searchBirthDateTo);
        }

        if (!string.IsNullOrEmpty(searchPlanet))
        {
            query = query.Where(c => c.Planet == searchPlanet);
        }

        if (!string.IsNullOrEmpty(searchGender))
        {
            query = query.Where(c => c.Gender == searchGender);
        }

        if (searchMovies != null && searchMovies.Split(",").Any())
        {
            query = query
                .Where(c => 
                c.Movies.Any(m => 
                searchMovies.Split(new char[]{','})
                .Contains(m.Title)));
        }
        return await query.CountAsync();
    }

    public async Task<int> GetTotalCharacterCountAsync()
    {
        return await _dbSet.CountAsync();
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