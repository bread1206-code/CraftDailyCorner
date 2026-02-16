using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorProduct;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorProductImageService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly IImageFileService _imageFileService;

        private const int MaxImageCount = 10;

        public CreatorProductImageService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService,
            IImageFileService imageFileService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _imageFileService = imageFileService;
        }

        // ================= 取得清單 =================

        public async Task<List<VMProductImageItem>> GetImagesAsync(
            string productId,
            string creatorId)
        {
            return await _context.ProductImages
                .Include(i => i.Product)
                .Where(i =>
                    i.ProductID == productId &&
                    i.Product.CreatorID == creatorId &&
                    i.StatusID == 1)
                .OrderBy(i => i.SortOrder)
                .Select(i => new VMProductImageItem
                {
                    ImageID = i.ImageID,
                    ImageUrl = i.ImageUrl,
                    SortOrder = i.SortOrder
                })
                .ToListAsync();
        }

        // ================= 上傳 =================

        public async Task UploadAsync(
            string productId,
            string creatorId,
            List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return;

            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductID == productId &&
                    p.CreatorID == creatorId);

            if (product == null)
                throw new Exception("找不到商品或無權限");

            var currentCount = await _context.ProductImages
                .CountAsync(i => i.ProductID == productId);

            if (currentCount + files.Count > MaxImageCount)
                throw new Exception($"商品最多只能上傳 {MaxImageCount} 張圖片");

            var maxSort = await _context.ProductImages
                .Where(i => i.ProductID == productId)
                .Select(i => (int?)i.SortOrder)
                .MaxAsync() ?? 0;

            foreach (var file in files)
            {
                var imageKey = _imageUploadService.UploadImage(
                    file,
                    null,
                    "04ProductImage",
                    ImageSizePresets.Product
                );

                _context.ProductImages.Add(new ProductImage
                {
                    ProductID = productId,
                    ImageUrl = imageKey,
                    SortOrder = (byte)(++maxSort),
                    StatusID = 1
                });
            }

            await _context.SaveChangesAsync();
        }

        // ================= 刪除 =================

        public async Task<string> DeleteAsync(long imageId, string creatorId)
        {
            var image = await _context.ProductImages
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.ImageID == imageId);

            if (image == null || image.Product.CreatorID != creatorId)
                throw new Exception("找不到圖片或無權限");

            var productId = image.ProductID;

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            await ReorderAsync(productId);
            await _context.SaveChangesAsync();

            return productId;
        }

        // ================= 批次排序 =================

        public async Task UpdateSortBatchAsync(
            List<ImageSortDTO> items,
            string creatorId)
        {
            var ids = items.Select(x => x.ImageID).ToList();

            var dbItems = await _context.ProductImages
                .Include(i => i.Product)
                .Where(i =>
                    ids.Contains(i.ImageID) &&
                    i.Product.CreatorID == creatorId)
                .ToListAsync();

            var dict = items.ToDictionary(x => x.ImageID, x => x.SortOrder);

            foreach (var item in dbItems)
            {
                if (dict.TryGetValue(item.ImageID, out var newSort))
                {
                    item.SortOrder = newSort;
                }
            }

            await _context.SaveChangesAsync();
        }

        // ================= 重新排序 =================

        private async Task ReorderAsync(string productId)
        {
            var items = await _context.ProductImages
                .Where(i => i.ProductID == productId)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            byte order = 1;

            foreach (var item in items)
            {
                item.SortOrder = order++;
            }
        }
    }
}