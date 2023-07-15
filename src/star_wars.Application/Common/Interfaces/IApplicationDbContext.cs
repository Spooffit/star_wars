using Microsoft.EntityFrameworkCore;
using star_wars.Core.Entities;

namespace star_wars.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Character> Characters { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}