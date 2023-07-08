using Microsoft.EntityFrameworkCore;
using star_wars.Application.Common.Interfaces;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Character> Characters { get; }
    public DbSet<Movie> Movies { get; }
    public DbSet<Planet> Planets { get; }

    public ApplicationDbContext(DbContextOptions options) :base(options){}

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}