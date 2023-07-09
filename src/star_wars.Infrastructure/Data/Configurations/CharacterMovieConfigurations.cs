using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Data.Configurations;

public class CharacterMovieConfigurations : IEntityTypeConfiguration<CharacterMovie>
{
    public void Configure(EntityTypeBuilder<CharacterMovie> builder)
    {
        builder.HasKey(cm => new { cm.Character, cm.Movie });
        builder.HasIndex(cm => cm.Movie);
        
        builder.HasOne(cm => cm.Character)
            .WithMany()
            .HasForeignKey(cm => cm.Character);
        
        builder.HasOne(cm => cm.Movie)
            .WithMany()
            .HasForeignKey(cm => cm.Movie);
    }
}