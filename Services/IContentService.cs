namespace RealEstatePortfolio.Services
{
    public interface IContentService
    {
        Task<string> GetContentAsync(string pageName, string sectionKey, string defaultValue = "");
        Task<Dictionary<string, string>> GetPageContentsAsync(string pageName);
        Task UpdateContentAsync(string pageName, string sectionKey, string contentValue);
    }
}
