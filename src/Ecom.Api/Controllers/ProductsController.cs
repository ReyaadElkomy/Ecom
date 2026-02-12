using AutoMapper;
using Ecom.Api.Helpers;
using Ecom.Core.DTOs;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers;

public class ProductsController : BaseController
{
    public ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(
                    x => x.Category, x => x.Photos
                );

            var result = _mapper.Map<List<ProductDto>>(products);

            if (products is null || !products.Any())
                return Ok(new ResponseApi(200, "No products found!"));

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _unitOfWork.ProductRepository
                .GetByIdAsync(id, x => x.Category, x => x.Photos);
            if (product is null)
                return NotFound(new ResponseApi(404, $"Product with id {id} not found!"));

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(CreateProductAddDto productDto)
    {
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(productDto);
            return Ok(new ResponseApi(200, "Product has been added!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update-product")]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto productDto)
    {
        try
        {
            await _unitOfWork.ProductRepository.UpdateAsync(productDto);
            return Ok(new ResponseApi(200, "Product has been updated!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            throw;
        }
    }


    [HttpDelete("delete-product/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _unitOfWork.ProductRepository
                .GetByIdAsync(id, x=>x.Category, x=> x.Photos);
            if (product is null)
                return NotFound(new ResponseApi(404, $"Product with id {id} not found!"));

            await _unitOfWork.ProductRepository.DeleteAsync(product);
            return Ok(new ResponseApi(200, "Product has been deleted!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}