using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        // builder.HasKey(c => c.Id);
        // builder.HasIndex(c => c.Id)
        //     .IsUnique();
        // builder.HasIndex(c => c.Gender);
        // builder.HasIndex(c => c.Birthdate);
        // builder.HasIndex(c => c.Species);
        // builder.Property(c => c.Id)
        //     .ValueGeneratedOnAdd();
        //
        // builder.Property(c => c.Name)
        //     .HasMaxLength(100)
        //     .IsRequired();
        // builder.Property(c => c.OriginName)
        //     .HasMaxLength(100)
        //     .IsRequired();
        // builder.Property(c => c.Birthdate)
        //     .IsRequired();
        // builder.Property(c => c.Gender)
        //     .HasMaxLength(30)
        //     .IsRequired();
        // builder.Property(c => c.Species)
        //     .IsRequired();
        // builder.Property(c => c.Height)
        //     .HasPrecision(10,2)
        //     .IsRequired();
        // builder.Property(c => c.HairColor)
        //     .HasMaxLength(30)
        //     .IsRequired();
        // builder.Property(c => c.EyeColor)
        //     .HasMaxLength(30)
        //     .IsRequired();
        // builder.Property(c => c.Description)
        //     .HasMaxLength(2000)
        //     .IsRequired();
        
        builder.HasMany(c => c.Movies)
            .WithMany(m => m.Characters)
            .UsingEntity("CharactersMovie");
    }
}