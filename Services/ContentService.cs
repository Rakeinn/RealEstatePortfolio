using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RealEstatePortfolio.Data;
using RealEstatePortfolio.Models.Entities;

namespace RealEstatePortfolio.Services
{
    public class ContentService : IContentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;
        private const string CachePrefix = "PageContent_";

        public ContentService(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<string> GetContentAsync(string pageName, string sectionKey, string defaultValue = "")
        {
            var cacheKey = $"{CachePrefix}{pageName}_{sectionKey}";
            if (_cache.TryGetValue(cacheKey, out string? cached) && cached != null)
            {
                return cached;
            }

            var item = await _db.PageContents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PageName.ToLower() == pageName.ToLower() && c.SectionKey.ToLower() == sectionKey.ToLower());

            var result = item?.ContentValue ?? defaultValue;
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        public async Task<Dictionary<string, string>> GetPageContentsAsync(string pageName)
        {
            var cacheKey = $"{CachePrefix}_ALL_{pageName}";
            if (_cache.TryGetValue(cacheKey, out Dictionary<string, string>? cachedDict) && cachedDict != null)
            {
                return cachedDict;
            }

            var items = await _db.PageContents
                .AsNoTracking()
                .Where(c => c.PageName.ToLower() == pageName.ToLower())
                .ToListAsync();

            var dict = items.ToDictionary(k => k.SectionKey, v => v.ContentValue ?? string.Empty);
            _cache.Set(cacheKey, dict, TimeSpan.FromMinutes(10));
            return dict;
        }

        public async Task UpdateContentAsync(string pageName, string sectionKey, string contentValue)
        {
            var item = await _db.PageContents
                .FirstOrDefaultAsync(c => c.PageName.ToLower() == pageName.ToLower() && c.SectionKey.ToLower() == sectionKey.ToLower());

            if (item == null)
            {
                _db.PageContents.Add(new PageContent
                {
                    PageName = pageName,
                    SectionKey = sectionKey,
                    ContentValue = contentValue
                });
            }
            else
            {
                item.ContentValue = contentValue;
            }

            await _db.SaveChangesAsync();

            // Invalidate cache
            _cache.Remove($"{CachePrefix}{pageName}_{sectionKey}");
            _cache.Remove($"{CachePrefix}_ALL_{pageName}");
        }
    }
}
