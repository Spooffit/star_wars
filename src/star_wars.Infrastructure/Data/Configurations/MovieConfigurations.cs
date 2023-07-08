using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class MovieConfigurations : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Id)
            .IsUnique();
        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Title)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasMany(m => m.Characters)
            .WithMany(c => c.Movies)
            .UsingEntity<CharacterMovie>();

    }
}