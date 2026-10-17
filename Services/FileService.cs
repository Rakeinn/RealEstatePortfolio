using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace RealEstatePortfolio.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string subFolder = "properties")
        {
            if (file == null || file.Length == 0) return string.Empty;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg", ".avif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid image file format. Allowed: JPG, PNG, WEBP, SVG, AVIF.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", subFolder);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/{subFolder}/{uniqueFileName}";
        }

        public async Task<string> SavePdfBrochureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return string.Empty;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".pdf")
            {
                throw new InvalidOperationException("Only PDF brochures are allowed.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "brochures");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid():N}.pdf";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/brochures/{uniqueFileName}";
        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return false;

            try
            {
                var cleanPath = relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(_env.WebRootPath, cleanPath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch
            {
                // Log and swallow file deletion errors
            }
            return false;
        }
    }
}
