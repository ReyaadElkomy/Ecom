using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Repositories;
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IImageManagementService _imageManagementService;
    public ProductRepository(ApplicationDbContext context, IMapper mapper, IImageManagementService imageManagementService = null) : base(context)
    {
        _context = context;
        _mapper = mapper;
        _imageManagementService = imageManagementService;
    }

    public async Task<bool> AddAsync(CreateProductAddDto createProductDto)
    {
        if(createProductDto is null)
            throw new ArgumentNullException(nameof(createProductDto));

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var product = _mapper.Map<Product>(createProductDto);
    
            var imagePath = await _imageManagementService.AddImageAsync(createProductDto.Photo, createProductDto.Name);
            product.Photos = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ImageUrl = path,
                ProductId = product.Id
            }).ToList();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
    {
        if (updateProductDto is null)
            throw new ArgumentNullException(nameof(updateProductDto));

        var product = await _context.Products
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.Id == updateProductDto.Id);

        if (product is null)
            throw new KeyNotFoundException($"Product with id {updateProductDto.Id} not found!");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1️⃣ رفع الصور الجديدة أولًا
            var newImagePaths = await _imageManagementService
                .AddImageAsync(updateProductDto.Photo, updateProductDto.Name);

            // 2️⃣ تحديث المنتج
            _mapper.Map(updateProductDto, product);

            // 3️⃣ حذف الصور القديمة من DB فقط
            var oldPhotos = product.Photos.ToList();
            _context.Photos.RemoveRange(oldPhotos);

            // 4️⃣ إضافة الجديدة
            product.Photos = newImagePaths.Select(path => new Photo
            {
                ImageName = path,
                ImageUrl = path
            }).ToList();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // 5️⃣ بعد نجاح DB نحذف الصور القديمة من السيرفر
            foreach (var photo in oldPhotos)
            {
                 _imageManagementService
                    .DeleteImage(photo.ImageName);
            }

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Product product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var photo in product.Photos)
            {
                _imageManagementService.DeleteImage(photo.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
