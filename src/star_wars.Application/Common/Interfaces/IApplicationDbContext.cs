using Microsoft.EntityFrameworkCore;
using star_wars.Core.Entities;

namespace star_wars.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Character> Characters { get; }
    DbSet<Movie> Movies { get; }
    DbSet<Planet> Planets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}