using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerEncyclopedia.Infrastructure.Data.Configuration
{
    public class StyleConfiguration : IEntityTypeConfiguration<Style>
    {
        public void Configure(EntityTypeBuilder<Style> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.NameRus).HasMaxLength(400);
            builder.Property(b => b.NameEn).HasMaxLength(400);
            builder.HasMany(m => m.Beers);
        }
    }
}
