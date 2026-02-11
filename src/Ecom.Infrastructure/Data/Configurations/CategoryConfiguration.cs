using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x=>x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" }
            );
    }
}
