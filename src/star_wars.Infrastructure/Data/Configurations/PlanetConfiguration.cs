using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class PlanetConfiguration : IEntityTypeConfiguration<Planet>
{
    public void Configure(EntityTypeBuilder<Planet> builder)
    {
        builder.HasKey(p => p.Name);
        builder.HasIndex(p => p.Name)
            .IsUnique();
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}