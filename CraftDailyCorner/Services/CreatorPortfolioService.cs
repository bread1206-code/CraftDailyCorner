using CraftDailyCorner.DTOs;
using CraftDailyCorner.ImageManagementCore.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.Reaction;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPortfolioService : ICreatorPortfolioService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly IReactionService _reactionService;

        public CreatorPortfolioService(CraftDailyCornerContext context, IImageUploadService imageUploadService, IReactionService reactionService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _reactionService = reactionService;
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
                    CreatorID = p.CreatorID,
                    ItemCount = p.PortfolioItems.Count(),

                    CoverImageUrl = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),

                    //內文預覽
                    Preview = p.Description,

                    // Reaction summary
                    ReactionSummary = new VMReactionButton
                    {
                        TargetType = ReactionTargetType.Portfolio,
                        TargetID = p.PortfolioID,

                        TotalCount = _context.Reactions.Count(r =>
                            r.TargetType == ReactionTargetType.Portfolio &&
                            r.TargetID == p.PortfolioID),

                        TopReactionType = _context.Reactions
                            .Where(r => r.TargetType == ReactionTargetType.Portfolio &&
                                        r.TargetID == p.PortfolioID)
                            .GroupBy(r => r.ReactionType)
                            .OrderByDescending(g => g.Count())
                            .ThenBy(g => (byte)g.Key)
                            .Select(g => (ReactionType?)g.Key)
                            .FirstOrDefault()
                    }
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
                string portfolioId, string? currentMemberId)
        {
            var data = await _context.Portfolios
                .Where(p =>
                    p.PortfolioID == portfolioId &&
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public)
                .Select(p => new
                {
                    p.PortfolioID,
                    p.Title,
                    p.Description,
                    p.CreatedAt,
                    CreatorName = p.CreatorProfile.DisplayName,
                    OwnerId = p.CreatorProfile.MemberID,
                    Items = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new VMPortfolioDetailItem
                        {
                            ItemID = i.ItemID,
                            ImageUrl = i.ImageUrl
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (data == null) return null;

            var reactionVm = await _reactionService.GetButtonStateAsync(
                currentMemberId,
                ReactionTargetType.Portfolio,
                data.PortfolioID);

            return new VMPortfolioDetail
            {
                PortfolioID = data.PortfolioID,
                Title = data.Title,
                Description = data.Description ?? "",
                CreatedAt = data.CreatedAt,
                CreatorName = data.CreatorName,
                IsOwner = currentMemberId != null && data.OwnerId == currentMemberId,
                Items = data.Items,
                ReactionButton = reactionVm
            };
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
                    Visibility = p.Visibility,
                    CoverImageUrl= p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        //編輯頁資料

        public async Task<VMCreatorPortfolioEdit?> GetEditDataAsync(string portfolioId, string creatorId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.PortfolioItems)
                .Where(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1)
                .FirstOrDefaultAsync();

            if (portfolio == null)
                return null;

            return new VMCreatorPortfolioEdit
            {
                PortfolioID = portfolio.PortfolioID,
                Title = portfolio.Title,
                Description = portfolio.Description ?? string.Empty,
                Visibility = portfolio.Visibility,
                UpdatedAt = portfolio.UpdatedAt,

                ImageManagement = new VMImageManagement
                {
                    EntityId = portfolio.PortfolioID,
                    EntityType = "Portfolio",
                    MaxImageCount = 25,
                    HintMessage = "作品圖片最多 25 張"
                }
            };
        }

        //建立

        public async Task CreateAsync(CreateCreatorPortfolioDTO dto,string creatorId,List<IFormFile> files)
        {
            var portfolioId = Guid.NewGuid().ToString();

            var portfolio = new Portfolio
            {
                PortfolioID = portfolioId,
                Title = dto.Title,
                Description = dto.Description,
                Visibility = dto.Visibility,
                CreatorID = creatorId,
                StatusID = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Portfolios.Add(portfolio);

            byte sort = 1;

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
                    SortOrder = sort++,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

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

            await ReorderPortfolioItems(dto.PortfolioID);
            await _context.SaveChangesAsync();
        }

        //軟刪除
        public async Task SoftDeleteAsync(
            string portfolioId,
            string creatorId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.PortfolioItems)
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (portfolio == null)
                throw new Exception("找不到作品或無權限");

            var now = DateTime.Now;

            //軟刪作品集
            portfolio.StatusID = 3; // Deleted
            portfolio.UpdatedAt = now;

            //軟刪所有圖片
            foreach (var item in portfolio.PortfolioItems)
            {
                if (!item.IsDeleted)
                {
                    item.IsDeleted = true;
                    item.DeletedAt = now;
                }
            }

            await _context.SaveChangesAsync();
        }
        private async Task ReorderPortfolioItems(string portfolioId)
        {
            var items = await _context.PortfolioItems
                .Where(i => i.PortfolioID == portfolioId)
                .OrderBy(i => i.SortOrder)
                .ToListAsync();

            byte order = 0;

            foreach (var item in items)
            {
                item.SortOrder = order++;
            }
        }
    }
}