using CraftDailyCorner.ImageManagementCore.Interfaces;
using CraftDailyCorner.ImageManagementCore.Services;
using CraftDailyCorner.ImageManagementCore.Services.Interfaces;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using Microsoft.EntityFrameworkCore;

public class PortfolioImageService
    : ImageManagementService<PortfolioItem>,
      IImageManagementService
{
    private readonly IImageUploadService _uploadService;

    public string EntityType => "Portfolio";
    public int? MaxImageCount => 25;
    public string? HintMessage => "作品圖片最多 25 張";

    public PortfolioImageService(
        CraftDailyCornerContext db,
        IImageUploadService uploadService)
        : base(db)
    {
        _uploadService = uploadService;
    }

    // =========================================================
    // 取得圖片（只抓未刪除）
    // =========================================================

    public override async Task<List<IEntityImage>> GetImagesAsync(string portfolioId)
    {
        var result = await _dbSet
            .Where(x => x.PortfolioID == portfolioId && !x.IsDeleted)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        return result.Cast<IEntityImage>().ToList();
    }

    // =========================================================
    // 新增圖片
    // =========================================================

    public async Task AddWithUploadAsync(
        IFormFile file,
        string portfolioId,
        string creatorId)
    {
        await ValidateOwnerAsync(portfolioId, creatorId);

        var currentCount = await _dbSet
            .Where(x => x.PortfolioID == portfolioId && !x.IsDeleted)
            .CountAsync();

        if (MaxImageCount.HasValue &&
            currentCount >= MaxImageCount.Value)
        {
            throw new InvalidOperationException(HintMessage);
        }

        string fileName = Guid.NewGuid().ToString();

        _uploadService.UploadImage(
            file,
            null,
            "06Portfolio",
            ImageSizePresets.Portfolio,
            fileName
        );

        var nextSort = await GetNextSortOrderAsync(
            x => x.PortfolioID == portfolioId && !x.IsDeleted);

        var item = new PortfolioItem
        {
            ImageUrl = fileName,
            PortfolioID = portfolioId,
            SortOrder = nextSort,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsDeleted = false
        };

        await AddEntityAsync(item);
    }

    // =========================================================
    // 軟刪除（含重排）
    // =========================================================

    public async Task DeleteWithValidationAsync(
        long imageId,
        string creatorId)
    {
        var item = await _dbSet
            .Include(x => x.Portfolio)
            .FirstOrDefaultAsync(x => x.ItemID == imageId);

        if (item == null || item.IsDeleted)
            throw new Exception("圖片不存在");

        if (item.Portfolio.CreatorID != creatorId)
            throw new UnauthorizedAccessException("無權限刪除");

        // 至少保留一張
        var count = await _dbSet
            .Where(x => x.PortfolioID == item.PortfolioID && !x.IsDeleted)
            .CountAsync();

        if (count <= 1)
            throw new InvalidOperationException("作品集至少需要一張圖片");

        item.IsDeleted = true;
        item.DeletedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;

        await ReorderAfterDelete(item.PortfolioID);

        await _db.SaveChangesAsync();
    }

    // =========================================================
    // 更新排序（只作用於未刪除）
    // =========================================================

    public async Task UpdateSortWithValidationAsync(
        string portfolioId,
        List<long> orderedIds,
        string creatorId)
    {
        await ValidateOwnerAsync(portfolioId, creatorId);

        var items = await _dbSet
            .Where(x => x.PortfolioID == portfolioId && !x.IsDeleted)
            .ToListAsync();

        await UpdateSortInternalAsync(
            items,
            orderedIds,
            (item, order) =>
            {
                item.SortOrder = order;
                item.UpdatedAt = DateTime.Now;
            });
    }

    // =========================================================
    // 重排排序
    // =========================================================

    private async Task ReorderAfterDelete(string portfolioId)
    {
        var items = await _dbSet
            .Where(x => x.PortfolioID == portfolioId && !x.IsDeleted)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SortOrder = (byte)(i + 1);
            items[i].UpdatedAt = DateTime.Now;
        }
    }

    // =========================================================
    // 驗證擁有者
    // =========================================================

    private async Task ValidateOwnerAsync(
        string portfolioId,
        string creatorId)
    {
        var exists = await _db.Portfolios
            .AnyAsync(p =>
                p.PortfolioID == portfolioId &&
                p.CreatorID == creatorId);

        if (!exists)
            throw new UnauthorizedAccessException("無權限操作此作品集");
    }
}