using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Configurations;
public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasData(
            new Photo
            {
                Id = 101,
                ImageUrl = "products/photo1.jpg",
                ImageName = "test 1",
                ProductId = 1
            }
            );
    }
}
