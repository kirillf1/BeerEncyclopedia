using BeerEncyclopedia.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeerEncyclopedia.Infrastructure.Data.Configuration
{
    public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.Id).IsUnique();
            builder.Property(m => m.Name).HasMaxLength(600);
            builder.HasIndex(m => m.Name);
            builder.HasOne(c => c.Country);
        }
    }
}
