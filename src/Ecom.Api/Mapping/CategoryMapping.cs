using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping;

public class CategoryMapping: Profile
{
    public CategoryMapping()
    {
        CreateMap<CategoryDto, Category>().ReverseMap();
    }
}
