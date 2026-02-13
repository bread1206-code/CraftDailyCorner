using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

public class CreatorPortfolioItemService
    : ICreatorPortfolioItemService
{
    private readonly CraftDailyCornerContext _context;
    private readonly IImageUploadService _imageUploadService;

    public CreatorPortfolioItemService(
        CraftDailyCornerContext context,
        IImageUploadService imageUploadService)
    {
        _context = context;
        _imageUploadService = imageUploadService;
    }

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

        var maxSort = await _context.PortfolioItems
            .Where(i => i.PortfolioID == portfolioId)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync() ?? 0;

        foreach (var file in files)
        {
            var imageKey = _imageUploadService.UploadImage(
                file,
                null,
                "06Portfolio",
                ImageSizePresets.Portfolio
            );

            _context.PortfolioItems.Add(new PortfolioItem
            {
                PortfolioID = portfolioId,
                ImageUrl = imageKey,
                SortOrder = (byte)(++maxSort),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
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

        if (item == null ||
            item.Portfolio.CreatorID != creatorId)
            throw new Exception("找不到圖片或無權限");

        var portfolioId = item.PortfolioID;

        _context.PortfolioItems.Remove(item);
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

        if (item == null ||
            item.Portfolio.CreatorID != creatorId)
            throw new Exception("找不到圖片或無權限");

        item.SortOrder = sortOrder;
        item.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}