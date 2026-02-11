using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IImageManagementService _imageManagementService;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IPhotoRepository PhotoRepository { get; }

    public UnitOfWork(ApplicationDbContext context, IMapper mapper, IImageManagementService imageManagementService)
    {
        _context = context;
        _mapper = mapper;
        _imageManagementService = imageManagementService;

        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context,_mapper, _imageManagementService);
        PhotoRepository = new PhotoRepository(_context);
       
    }
}
