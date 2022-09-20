using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopParsers;

namespace ShopBeerService.Infrastructure.EntityConfigurations
{
    public class ShopBeerConfiguration : IEntityTypeConfiguration<ShopBeer>
    {
        public void Configure(EntityTypeBuilder<ShopBeer> builder)
        {
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            builder.HasKey(b => b.Id);
            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.FormatedName)
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex("english");
        }
    }
}
