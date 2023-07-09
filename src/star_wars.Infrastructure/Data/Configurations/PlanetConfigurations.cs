using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class PlanetConfigurations : IEntityTypeConfiguration<Planet>
{
    public void Configure(EntityTypeBuilder<Planet> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Id)
            .IsUnique();
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasMany(p => p.Characters)
            .WithOne(c => c.Planet)
            .HasForeignKey(c => c.Planet);
    }
}