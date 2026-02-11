using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(des=>des.CategoryName,opt=>opt.MapFrom(src=>src.Category.Name))
            .ReverseMap();
        CreateMap<PhotoDto, Photo>().ReverseMap();

        CreateMap<CreateProductAddDto, Product>()
            .ForMember(des => des.Photos, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<UpdateProductDto, Product>()
            .ForMember(des => des.Photos, opt => opt.Ignore())
            .ReverseMap();
    }
}
