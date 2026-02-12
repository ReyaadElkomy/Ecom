using System.ComponentModel.DataAnnotations.Schema;
using Ecom.Core.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Ecom.Core.DTOs;
public record ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }
    public virtual List<PhotoDto> Photos { get; set; }
    public string CategoryName { get; set; }
}

public record PhotoDto
{
    public string ImageName { get; set; }
    public string ImageUrl { get; set; }
    public int ProductId { get; set; }
}

public record CreateProductAddDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }
    public int CategoryId { get; set; }
    public IFormFileCollection Photo { get; set; }
}

public record UpdateProductDto : CreateProductAddDto
{
    public int Id { get; set; }
}
