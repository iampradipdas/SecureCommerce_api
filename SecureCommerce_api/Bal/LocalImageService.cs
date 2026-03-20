using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SecureCommerce_api.Bal.Interfaces;

namespace SecureCommerce_api.Bal
{
    public class LocalImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var uploadsFolder = Path.Combine(rootPath, folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return $"{baseUrl}/{folder}/{fileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                var uri = new Uri(imageUrl);
                var fileName = Path.GetFileName(uri.LocalPath);
                var folder = Path.GetDirectoryName(uri.LocalPath)?.TrimStart('/');
                
                if (folder != null)
                {
                    var rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var filePath = Path.Combine(rootPath, folder, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch
            {
                // Ignore errors during deletion
            }
        }
    }
}
