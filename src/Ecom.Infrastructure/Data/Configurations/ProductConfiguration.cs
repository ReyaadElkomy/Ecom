using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.HasData(
            new Product { Id = 1, Name = "Smartphone", Description = "Latest model smartphone with advanced features", Price = 699.99m, CategoryId = 1 },
                 new Product { Id = 2, Name = "Laptop", Description = "High-performance laptop for work and gaming", Price = 1299.99m, CategoryId = 1 }
        );
    }
}
