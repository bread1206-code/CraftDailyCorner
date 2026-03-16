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

            if (!seedContext.CreatorPortfolioMap.Any())
                return;

            var portfolioFolder = GetPortfolioSourceFolder();

            var allFiles = _imageHelper.GetOrderedFiles(portfolioFolder)
                .Where(IsSupportedImageFile)
                .ToList();

            if (!allFiles.Any())
                return;

            if (!seedContext.BrandCodeToCreatorMap.Any())
                throw new Exception("BrandCodeToCreatorMap 沒有資料，請先執行 DemoSeedPortfolios");

            var portfolioCreatedAtMap = _context.Portfolios
                .AsNoTracking()
                .Select(x => new
                {
                    x.PortfolioID,
                    x.CreatedAt
                })
                .ToDictionary(x => x.PortfolioID, x => x.CreatedAt);

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

                // 規則：只有本次新建立的 Portfolio 才建立 PortfolioItem
                if (!seedContext.CreatorPortfolioMap.TryGetValue(creatorId, out var portfolioId))
                    continue;

                if (!portfolioCreatedAtMap.TryGetValue(portfolioId, out var createdAt))
                    throw new Exception($"找不到 Portfolio.CreatedAt：{portfolioId}");

                var orderedFiles = group
                    .OrderBy(Path.GetFileName)
                    .ToList();

                byte sortOrder = 1;

                foreach (var file in orderedFiles)
                {
                    if (existingItemKeys.Contains(new { PortfolioID = portfolioId, SortOrder = sortOrder }))
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
            return Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "Portfolio");
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