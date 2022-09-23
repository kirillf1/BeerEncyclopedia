using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerEncyclopedia.Infrastructure.Data.Configuration
{
    public class BeerConfiguration : IEntityTypeConfiguration<Beer>
    {
        public void Configure(EntityTypeBuilder<Beer> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.Id).IsUnique();
            builder.HasIndex(b => new { b.Name, b.AltName })
                .HasMethod("GIN")
                 .IsTsVectorExpressionIndex("english")
                .IsTsVectorExpressionIndex("russian");
            builder.OwnsOne(b => b.BeerImages)
                .ToTable("BeerImages");
            builder.HasOne(b => b.Country);
            builder.HasMany(b => b.Manufacturers);
            builder.OwnsOne(b => b.ChemicalIndicators)
                .ToTable("ChemicalIndicators");
            builder.OwnsOne(b => b.OrganolepticIndicators).
                ToTable("OrganolepticIndicators").
                HasOne(o => o.Color);
        }
    }
}
