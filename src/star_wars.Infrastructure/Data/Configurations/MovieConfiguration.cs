using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Id)
            .IsUnique();
        builder.HasIndex(m => m.Title)
            .IsUnique();
        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Title)
            .HasMaxLength(100)
            .IsRequired();
    }
}