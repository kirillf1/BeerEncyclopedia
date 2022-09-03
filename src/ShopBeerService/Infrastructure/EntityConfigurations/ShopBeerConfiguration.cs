using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopParsers;

namespace ShopBeerService.Infrastructure.EntityConfigurations
{
    public class ShopBeerConfiguration : IEntityTypeConfiguration<ShopBeer>
    {
        public void Configure(EntityTypeBuilder<ShopBeer> builder)
        {
            builder.Property<int>("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.HasIndex(c => c.Name);
        }
    }
}
