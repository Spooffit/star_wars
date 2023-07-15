using Microsoft.EntityFrameworkCore;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _db;
    private readonly DbSet<Movie> _dbSet;

    public MovieRepository(
        ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<Movie>();
    }
    
    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await _dbSet.ToListAsync();
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}