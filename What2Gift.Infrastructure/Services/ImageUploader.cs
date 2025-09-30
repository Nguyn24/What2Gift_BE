using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using What2Gift.Application.Abstraction.Authentication;

namespace What2Gift.Infrastructure.Services;

public class ImageUploader : IImageUploader
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageUploader(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folderName)
    {
        // ✅ Fallback nếu WebRootPath null (bị lỗi như log mày gửi)
        var rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var uploadsFolder = Path.Combine(rootPath, folderName);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(fileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(stream);

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = request != null
            ? $"{request.Scheme}://{request.Host}"
            : "";

        return $"{baseUrl}/{folderName}/{uniqueFileName}";
    }
}