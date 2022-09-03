using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopParsers;

namespace ShopBeerService.Infrastructure.EntityConfigurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(c => c.ShopBeers);
        }
    }
}
