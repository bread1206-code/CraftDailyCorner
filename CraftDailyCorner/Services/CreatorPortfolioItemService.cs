using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPortfolioItemService : ICreatorPortfolioItemService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly IImageFileService _imageFileService;

        public CreatorPortfolioItemService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService,
            IImageFileService imageFileService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _imageFileService = imageFileService;
        }

        private const int MaxImageCount = 25;

        public async Task UploadAsync(
            string portfolioId,
            string creatorId,
            List<IFormFile> files)
        {
            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (portfolio == null)
                throw new Exception("找不到作品集或無權限");

            var currentCount = await _context.PortfolioItems
                .CountAsync(i => i.PortfolioID == portfolioId && !i.IsDeleted);

            if (currentCount + files.Count > MaxImageCount)
                throw new Exception($"作品集最多只能上傳 {MaxImageCount} 張圖片");

            var maxSort = await _context.PortfolioItems
                .Where(i => i.PortfolioID == portfolioId && !i.IsDeleted)
                .MaxAsync(i => (byte?)i.SortOrder) ?? (byte)0;

            foreach (var file in files)
            {
                var imageKey = _imageUploadService.UploadImage(
                    file,
                    null,
                    "06Portfolio",
                    ImageSizePresets.Portfolio,
                    entityId: null,
                    entitySubFolder: creatorId
                );

                maxSort = (byte)(maxSort + 1);

                _context.PortfolioItems.Add(new PortfolioItem
                {
                    PortfolioID = portfolioId,
                    ImageUrl = imageKey,
                    SortOrder = maxSort,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteAsync(
            int itemId,
            string creatorId)
        {
            var item = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .FirstOrDefaultAsync(i => i.ItemID == itemId);

            if (item == null || item.Portfolio.CreatorID != creatorId)
                throw new Exception("找不到圖片或無權限");

            var portfolioId = item.PortfolioID;

            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;

            await ReorderAsync(portfolioId);
            await _context.SaveChangesAsync();

            return portfolioId;
        }

        public async Task UpdateSortAsync(
            int itemId,
            byte sortOrder,
            string creatorId)
        {
            var item = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .FirstOrDefaultAsync(i => i.ItemID == itemId);

            if (item == null || item.Portfolio.CreatorID != creatorId)
                throw new Exception("找不到圖片或無權限");

            if (sortOrder < 1)
                throw new Exception("排序值不可小於 1");

            item.SortOrder = sortOrder;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateSortBatchAsync(
            List<SortUpdateDTO> items,
            string creatorId)
        {
            var itemIds = items.Select(x => x.ItemId).ToList();

            var dbItems = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .Where(i => itemIds.Contains(i.ItemID)
                            && i.Portfolio.CreatorID == creatorId)
                .ToListAsync();

            var sortDict = items.ToDictionary(x => x.ItemId, x => x.SortOrder);

            foreach (var dbItem in dbItems)
            {
                if (sortDict.TryGetValue(dbItem.ItemID, out var newSort))
                {
                    dbItem.SortOrder = newSort;
                    dbItem.UpdatedAt = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task ReorderAsync(string portfolioId)
        {
            var items = await _context.PortfolioItems
                .Where(i => i.PortfolioID == portfolioId && !i.IsDeleted)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            int order = 1;

            foreach (var item in items)
            {
                item.SortOrder = (byte)order++;
            }
        }
    }
}