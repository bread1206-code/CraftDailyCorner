using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedPortfolioItems
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedPortfolioItems(
            CraftDailyCornerContext context,
            DemoSeedImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            var portfolioFolder = GetPortfolioSourceFolder();

            var allFiles = _imageHelper.GetOrderedFiles(portfolioFolder)
                .Where(IsSupportedImageFile)
                .ToList();

            if (!allFiles.Any())
                return;

            if (!seedContext.BrandCodeToCreatorMap.Any())
                throw new Exception("BrandCodeToCreatorMap 沒有資料，請先執行 DemoSeedPortfolios");

            // 改成直接從 DB 撈所有 Portfolio，而不是只看本次新建的 CreatorPortfolioMap
            var portfolioMap = _context.Portfolios
                .AsNoTracking()
                .Select(x => new
                {
                    x.CreatorID,
                    x.PortfolioID,
                    x.CreatedAt
                })
                .ToDictionary(x => x.CreatorID, x => new
                {
                    x.PortfolioID,
                    x.CreatedAt
                });

            if (!portfolioMap.Any())
                return;

            var existingItemKeys = _context.PortfolioItems
                .AsNoTracking()
                .Select(x => new { x.PortfolioID, x.SortOrder })
                .ToHashSet();

            var brandGroups = allFiles
                .GroupBy(x => GetBrandCodeFromFileName(x))
                .OrderBy(g => g.Key)
                .ToList();

            var items = new List<PortfolioItem>();

            foreach (var group in brandGroups)
            {
                var brandCode = group.Key;

                if (!seedContext.BrandCodeToCreatorMap.TryGetValue(brandCode, out var creatorId))
                    throw new Exception($"找不到品牌代碼對應的 CreatorID：{brandCode}");

                if (!portfolioMap.TryGetValue(creatorId, out var portfolioInfo))
                    continue;

                var portfolioId = portfolioInfo.PortfolioID;
                var createdAt = portfolioInfo.CreatedAt;

                var orderedFiles = group
                    .OrderBy(Path.GetFileName)
                    .ToList();

                byte sortOrder = 1;

                foreach (var file in orderedFiles)
                {
                    var key = new
                    {
                        PortfolioID = portfolioId,
                        SortOrder = sortOrder
                    };

                    if (existingItemKeys.Contains(key))
                    {
                        sortOrder++;
                        continue;
                    }

                    var imageGuid = _imageHelper.UploadPortfolioImage(
                        sourceFilePath: file,
                        creatorId: creatorId);

                    items.Add(new PortfolioItem
                    {
                        ImageUrl = imageGuid,
                        SortOrder = sortOrder,
                        CreatedAt = createdAt,
                        UpdatedAt = createdAt,
                        PortfolioID = portfolioId,
                        IsDeleted = false,
                        DeletedAt = null
                    });

                    sortOrder++;
                }
            }

            if (items.Any())
            {
                _context.PortfolioItems.AddRange(items);
                _context.SaveChanges();
            }
        }

        private static string GetPortfolioSourceFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Seed","SeedAssets", "Portfolio");
        }

        private static string GetBrandCodeFromFileName(string filePath)
        {
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

            if (string.IsNullOrWhiteSpace(fileNameWithoutExt) || fileNameWithoutExt.Length < 3)
                throw new Exception($"作品集圖片檔名格式錯誤：{fileNameWithoutExt}");

            return fileNameWithoutExt[..3];
        }

        private static bool IsSupportedImageFile(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return ext is ".png" or ".jpg" or ".jpeg" or ".webp";
        }
    }
}