using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories.Services;
public class ImageManagementService : IImageManagementService
{
    private readonly IFileProvider _fileProvider;
    public ImageManagementService(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
    {
        List<string> SaveImageSrc = new List<string>();

        if(string.IsNullOrWhiteSpace(src) || src.Contains(".."))
            throw new ArgumentException("Invalid source path!");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        const long maxFileSize = 5 * 1024 * 1024; // 5 MB

        var ImageDirectory = Path.Combine("wwwroot","Images", src);
        if(!Directory.Exists(ImageDirectory))
            Directory.CreateDirectory(ImageDirectory);
        
        foreach (var file in files)
        {
            if(file.Length == 0)
                continue;

            if(file.Length > maxFileSize)
                throw new ArgumentException($"File {file.FileName} exceeds the maximum allowed size of 5 MB!");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(!allowedExtensions.Contains(extension))
                throw new ArgumentException($"File {file.FileName} has an unsupported file type!");

          
            var imageName = $"{Guid.NewGuid()}{extension}";
            var imageSrc = $"/Images/{src}/{imageName}";
            var imagePath = Path.Combine(ImageDirectory, imageName);
           
            await using var stream = new FileStream(
                imagePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                8192,
                useAsync: true);

            await file.CopyToAsync(stream);
            SaveImageSrc.Add(imageSrc);
        }
        return SaveImageSrc;
    }

    public void DeleteImage(string src)
    {
        var info = _fileProvider.GetFileInfo(src);
        if(info.Exists)
        {
            var filePath = info.PhysicalPath;
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
