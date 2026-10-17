using Microsoft.AspNetCore.Http;

namespace RealEstatePortfolio.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file, string subFolder = "properties");
        Task<string> SavePdfBrochureAsync(IFormFile file);
        bool DeleteFile(string relativePath);
    }
}
