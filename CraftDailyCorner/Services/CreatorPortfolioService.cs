using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPortfolioService : ICreatorPortfolioService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorPortfolioService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        //前台 Index（搜尋 + 分頁）

        public async Task<VMPortfolioIndex> GetPortfolioIndexAsync(
            VMPortfolioIndexQuery query)
        {
            var baseQuery = _context.Portfolios
                .Where(p =>
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Title.Contains(query.Keyword) ||
                    p.Description != null &&
                    p.Description.Contains(query.Keyword));
            }

            var totalCount = await baseQuery.CountAsync();

            var portfolios = await baseQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new VMCreatorPortfolioPublicListItem
                {
                    PortfolioID = p.PortfolioID,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt,
                    CreatorName = p.CreatorProfile.DisplayName,
                    ItemCount = p.PortfolioItems.Count()
                })
                .ToListAsync();

            return new VMPortfolioIndex
            {
                Query = query,
                Portfolios = portfolios,
                TotalCount = totalCount
            };
        }

        //前台 Detail

        public async Task<VMPortfolioDetail?> GetPublicPortfolioDetailAsync(
            string portfolioId)
        {
            return await _context.Portfolios
                .Where(p =>
                    p.PortfolioID == portfolioId &&
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public)
                .Select(p => new VMPortfolioDetail
                {
                    PortfolioID = p.PortfolioID,
                    Title = p.Title,
                    Description = p.Description!,
                    CreatedAt = p.CreatedAt,
                    CreatorName = p.CreatorProfile.DisplayName,
                    Items = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new VMPortfolioDetailItem
                        {
                            ItemID = i.ItemID,
                            ImageUrl = i.ImageUrl
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        //後台列表

        public async Task<List<VMCreatorPortfolioListItem>>
            GetCreatorPortfoliosAsync(string creatorId)
        {
            return await _context.Portfolios
                .Where(p => p.CreatorID == creatorId && p.StatusID == 1)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMCreatorPortfolioListItem
                {
                    PortfolioID = p.PortfolioID,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    ItemCount = p.PortfolioItems.Count(),
                    Visibility = p.Visibility
                })
                .ToListAsync();
        }

        //編輯頁資料

        public async Task<VMCreatorPortfolioEdit?>
            GetEditDataAsync(string portfolioId, string creatorId)
        {
            return await _context.Portfolios
                .Where(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1)
                .Select(p => new VMCreatorPortfolioEdit
                {
                    PortfolioID = p.PortfolioID,
                    Title = p.Title,
                    Description = p.Description!,
                    Visibility = p.Visibility,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        //建立

        public async Task CreateAsync(
            CreateCreatorPortfolioDTO dto,
            string creatorId)
        {
            var entity = new Portfolio
            {
                PortfolioID = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Description = dto.Description,
                Visibility = dto.Visibility,
                CreatorID = creatorId,
                StatusID = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Portfolios.Add(entity);
            await _context.SaveChangesAsync();
        }

        //更新

        public async Task UpdateAsync(
            UpdateCreatorPortfolioDTO dto,
            string creatorId)
        {
            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == dto.PortfolioID &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (portfolio == null)
                throw new Exception("找不到作品或無權限");

            portfolio.Title = dto.Title;
            portfolio.Description = dto.Description;
            portfolio.Visibility = dto.Visibility;
            portfolio.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        //軟刪除

        public async Task SoftDeleteAsync(
            string portfolioId,
            string creatorId)
        {
            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (portfolio == null)
                throw new Exception("找不到作品或無權限");

            portfolio.StatusID = 2;
            portfolio.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}