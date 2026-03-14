using CraftDailyCorner.ImageManagementCore.Interfaces;
using CraftDailyCorner.ImageManagementCore.Services.Interfaces;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftDailyCorner.ImageManagementCore.Services
{
    public class ProductImageService
        : ImageManagementService<ProductImage>,
          IImageManagementService
    {
        private readonly IImageUploadService _uploadService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProductImageService> _logger;

        public string EntityType => "Product";
        public int? MaxImageCount => 10;
        public string? HintMessage => "商品圖片最多 10 張";

        public ProductImageService(
            CraftDailyCornerContext db,
            IImageUploadService uploadService,
            IWebHostEnvironment env,
            ILogger<ProductImageService> logger)
            : base(db)
        {
            _uploadService = uploadService;
            _env = env;
            _logger = logger;
        }

        // =========================================================
        // 取得圖片（載入 Product）
        // =========================================================
        public override async Task<List<IEntityImage>> GetImagesAsync(string productId)
        {
            var result = await _dbSet
                .Include(x => x.Product)
                .Where(x => x.ProductID == productId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return result.Cast<IEntityImage>().ToList();
        }

        // =========================================================
        // 新增圖片
        // =========================================================
        public async Task AddWithUploadAsync(
            IFormFile file,
            string productId,
            string creatorId)
        {
            await ValidateOwnerAsync(productId, creatorId);

            var currentCount = await _dbSet
                .Where(x => x.ProductID == productId)
                .CountAsync();

            if (MaxImageCount.HasValue && currentCount >= MaxImageCount.Value)
            {
                throw new InvalidOperationException(HintMessage);
            }

            string fileName = Guid.NewGuid().ToString();

            _uploadService.UploadImage(
                file,
                null,
                "04ProductImage",
                ImageSizePresets.Product,
                entityId: fileName,
                entitySubFolder: creatorId
            );

            var nextSort = await GetNextSortOrderAsync(
                x => x.ProductID == productId);

            var image = new ProductImage
            {
                ImageUrl = fileName,
                ProductID = productId,
                SortOrder = nextSort,
                StatusID = 1
            };

            await AddEntityAsync(image);
        }

        // =========================================================
        // 安全硬刪（DB成功才刪檔）
        // =========================================================
        public async Task DeleteWithValidationAsync(
            long imageId,
            string creatorId)
        {
            var image = await _dbSet
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ImageID == imageId);

            if (image == null)
                throw new Exception("圖片不存在");

            if (image.Product.CreatorID != creatorId)
                throw new UnauthorizedAccessException("無權限刪除");

            var productId = image.ProductID;

            var imageCount = await _dbSet
                .Where(x => x.ProductID == productId)
                .CountAsync();

            if (imageCount <= 1)
                throw new InvalidOperationException("商品至少需要一張圖片");

            var fileName = image.ImageUrl;

            _dbSet.Remove(image);

            await ReorderAfterDelete(productId);
            await _db.SaveChangesAsync();

            try
            {
                DeletePhysicalFiles(creatorId, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "刪除商品圖片實體檔案失敗，CreatorID: {CreatorID}, FileName: {FileName}",
                    creatorId,
                    fileName
                );
            }
        }

        // =========================================================
        // 更新排序
        // =========================================================
        public async Task UpdateSortWithValidationAsync(
            string productId,
            List<long> orderedIds,
            string creatorId)
        {
            await ValidateOwnerAsync(productId, creatorId);

            var images = await _dbSet
                .Where(x => x.ProductID == productId)
                .ToListAsync();

            await UpdateSortInternalAsync(
                images,
                orderedIds,
                (image, order) =>
                {
                    image.SortOrder = order;
                });
        }

        // =========================================================
        // 驗證擁有者
        // =========================================================
        private async Task ValidateOwnerAsync(
            string productId,
            string creatorId)
        {
            var exists = await _db.Products
                .AnyAsync(p =>
                    p.ProductID == productId &&
                    p.CreatorID == creatorId);

            if (!exists)
                throw new UnauthorizedAccessException("無權限操作此商品");
        }

        // =========================================================
        // 刪除實體檔案
        // =========================================================
        private void DeletePhysicalFiles(string creatorId, string fileName)
        {
            string basePath = Path.Combine(
                _env.WebRootPath,
                "Photos",
                "04ProductImage",
                creatorId
            );

            string[] folders = { "Medium", "Large" };

            foreach (var folder in folders)
            {
                var path = Path.Combine(basePath, folder, $"{fileName}.png");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        // =========================================================
        // 重排排序
        // =========================================================
        private async Task ReorderAfterDelete(string productId)
        {
            var images = await _dbSet
                .Where(x => x.ProductID == productId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            for (int i = 0; i < images.Count; i++)
            {
                images[i].SortOrder = (byte)(i + 1);
            }
        }
    }
}