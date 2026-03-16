using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedPortfolios
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedPortfolios(
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

            EnsureBrandCodeToCreatorMap(seedContext);

            var creatorProfileMap = _context.CreatorProfiles
                .AsNoTracking()
                .Select(x => new
                {
                    x.CreatorID,
                    x.BrandName
                })
                .ToDictionary(x => x.CreatorID, x => x.BrandName);

            var existingPortfolioCreatorIds = _context.Portfolios
                .AsNoTracking()
                .Select(x => x.CreatorID)
                .ToHashSet();

            var brandGroups = allFiles
                .GroupBy(x => GetBrandCodeFromFileName(x))
                .OrderBy(g => g.Key)
                .ToList();

            var portfolios = new List<Portfolio>();

            foreach (var group in brandGroups)
            {
                var brandCode = group.Key;

                if (!seedContext.BrandCodeToCreatorMap.TryGetValue(brandCode, out var creatorId))
                    throw new Exception($"找不到品牌代碼對應的 CreatorID：{brandCode}");

                if (existingPortfolioCreatorIds.Contains(creatorId))
                    continue;

                if (!creatorProfileMap.TryGetValue(creatorId, out var brandName))
                    throw new Exception($"找不到 CreatorProfile：{creatorId}");

                if (!int.TryParse(brandCode, out var brandNo))
                    throw new Exception($"品牌代碼不是有效數字：{brandCode}");

                var createdAt = DemoSeedPortfolioTimeHelper.GetPortfolioCreatedAt(brandNo);
                var portfolioId = Guid.NewGuid().ToString();

                portfolios.Add(new Portfolio
                {
                    PortfolioID = portfolioId,
                    Title = brandName,
                    Description = string.Empty,
                    Visibility = CreatorVisibility.Public,
                    StatusID = 1,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt,
                    CreatorID = creatorId
                });

                seedContext.CreatorPortfolioMap[creatorId] = portfolioId;
            }

            if (portfolios.Any())
            {
                _context.Portfolios.AddRange(portfolios);
                _context.SaveChanges();
            }
        }

        private void EnsureBrandCodeToCreatorMap(DemoSeedContext seedContext)
        {
            if (seedContext.BrandCodeToCreatorMap.Any())
                return;

            var creatorBrandFolder = GetCreatorBrandSourceFolder();

            var creatorBrandFiles = _imageHelper.GetOrderedFiles(creatorBrandFolder)
                .Where(IsSupportedImageFile)
                .ToList();

            if (!creatorBrandFiles.Any())
                throw new Exception("CreatorBrand 資料夾沒有任何圖片，無法建立 BrandCodeToCreatorMap");

            var creatorProfiles = _context.CreatorProfiles
                .AsNoTracking()
                .Select(x => new
                {
                    x.CreatorID,
                    x.BrandName
                })
                .ToList();

            var brandNameToCreatorMap = creatorProfiles
                .GroupBy(x => x.BrandName)
                .ToDictionary(g => g.Key, g => g.First().CreatorID);

            foreach (var file in creatorBrandFiles)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);

                if (string.IsNullOrWhiteSpace(fileNameWithoutExt) || fileNameWithoutExt.Length < 4)
                    throw new Exception($"CreatorBrand 檔名格式錯誤：{fileNameWithoutExt}");

                var brandCode = fileNameWithoutExt[..3];
                var brandName = fileNameWithoutExt[3..].Trim();

                if (string.IsNullOrWhiteSpace(brandName))
                    throw new Exception($"CreatorBrand 檔名缺少品牌名稱：{fileNameWithoutExt}");

                if (!brandNameToCreatorMap.TryGetValue(brandName, out var creatorId))
                    throw new Exception($"CreatorBrand 檔名中的品牌名稱找不到對應 CreatorProfile：{brandName}");

                seedContext.BrandCodeToCreatorMap[brandCode] = creatorId;
            }
        }

        private static string GetPortfolioSourceFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "Portfolio");
        }

        private static string GetCreatorBrandSourceFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "CreatorBrand");
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