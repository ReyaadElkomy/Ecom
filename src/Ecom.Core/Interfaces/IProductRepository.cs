using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Shares;

namespace Ecom.Core.Interfaces;
public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<ProductDto>> GetAllSortedByPriceAsync(ProductParams productParams);
    Task<bool> AddAsync(CreateProductAddDto createProductDto);
    Task<bool> UpdateAsync(UpdateProductDto updateProductDto);
    Task DeleteAsync(Product product);
}
