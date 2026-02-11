using AutoMapper;
using Ecom.Api.Helpers;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers;

public class CategoriesController : BaseController
{
    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (categories is null || !categories.Any())
                return NotFound(new ResponseApi(404));
            return Ok(categories);
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
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound(new ResponseApi(404, $"Category with id {id} not found!"));
            return Ok(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            return Ok(new ResponseApi(200, "Category has been added!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update-category/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
    {
        try
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound(new ResponseApi(400));

            category = _mapper.Map(categoryDto, category);
            await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return Ok(new ResponseApi(200, "Category has beed updated!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete-category/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound(new ResponseApi(400));

            await _unitOfWork.CategoryRepository.DeleteAsync(id);
            return Ok(new ResponseApi(200, "Category has beed deleted!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
