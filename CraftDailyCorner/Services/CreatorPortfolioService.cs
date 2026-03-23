using CraftDailyCorner.DTOs;
using CraftDailyCorner.ImageManagementCore.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.Reaction;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPortfolioService : ICreatorPortfolioService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly IReactionService _reactionService;
        private readonly INotificationService _notificationService;

        public CreatorPortfolioService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService,
            IReactionService reactionService,
            INotificationService notificationService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _reactionService = reactionService;
            _notificationService = notificationService;
        }

        public async Task<VMPortfolioIndex> GetPortfolioIndexAsync(
            VMPortfolioIndexQuery query,
            string? currentMemberId)
        {
            var baseQuery = _context.Portfolios
                .AsNoTracking()
                .Where(p => p.StatusID == 1);

            if (!string.IsNullOrWhiteSpace(query.PortfolioKeyword))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Title.Contains(query.PortfolioKeyword) ||
                    (p.Description != null && p.Description.Contains(query.PortfolioKeyword)));
            }

            baseQuery = baseQuery.Where(p =>
                // 一般訪客：只能看公開
                p.Visibility == CreatorVisibility.Public

                // 作者本人：可看自己的全部作品集
                || (currentMemberId != null && p.CreatorProfile.MemberID == currentMemberId)

                // 已追蹤會員：可看追蹤者限定
                || (
                    p.Visibility == CreatorVisibility.Followers &&
                    currentMemberId != null &&
                    _context.FollowCreators.Any(f =>
                        f.CreatorID == p.CreatorID &&
                        f.MemberID == currentMemberId)
                )
            );

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
                    CreatorName = p.CreatorProfile.BrandName,
                    CreatorID = p.CreatorID,
                    ItemCount = p.PortfolioItems.Count(),
                    Visibility = p.Visibility,

                    CoverImageUrl = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),

                    Preview = p.Description,

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

        public async Task<VMPortfolioDetail?> GetPublicPortfolioDetailAsync(
            string portfolioId,
            string? currentMemberId)
        {
            var data = await _context.Portfolios
                .Where(p =>
                    p.PortfolioID == portfolioId &&
                    p.StatusID == 1)
                .Select(p => new
                {
                    p.PortfolioID,
                    p.Title,
                    p.Description,
                    p.CreatedAt,
                    CreatorName = p.CreatorProfile.BrandName,
                    OwnerId = p.CreatorProfile.MemberID,
                    CreatorID = p.CreatorID,
                    Items = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new VMPortfolioDetailItem
                        {
                            ItemID = i.ItemID,
                            ImageUrl = i.ImageUrl,
                            CreatorID = p.CreatorID
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (data == null) return null;

            var reactionVm = await _reactionService.GetButtonStateAsync(
                currentMemberId,
                ReactionTargetType.Portfolio,
                data.PortfolioID);

            bool isReportBanned = false;
            DateTime? reportBanUntil = null;

            if (!string.IsNullOrWhiteSpace(currentMemberId))
            {
                var member = await _context.Members
                    .AsNoTracking()
                    .Where(m => m.MemberID == currentMemberId)
                    .Select(m => new
                    {
                        m.ReportBanUntil
                    })
                    .FirstOrDefaultAsync();

                if (member != null)
                {
                    reportBanUntil = member.ReportBanUntil;
                    isReportBanned = member.ReportBanUntil.HasValue &&
                                     member.ReportBanUntil.Value > DateTime.Now;
                }
            }

            return new VMPortfolioDetail
            {
                PortfolioID = data.PortfolioID,
                Title = data.Title,
                Description = data.Description ?? "",
                CreatedAt = data.CreatedAt,
                CreatorName = data.CreatorName,
                IsOwner = currentMemberId != null && data.OwnerId == currentMemberId,
                Items = data.Items,
                ReactionButton = reactionVm,
                CreatorID = data.CreatorID,
                IsReportBanned = isReportBanned,
                ReportBanUntil = reportBanUntil
            };
        }

        public async Task<bool> CanViewPortfolioAsync(string portfolioId, string? memberId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.CreatorProfile)
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == portfolioId &&
                    p.StatusID == 1);

            if (portfolio == null)
                return false;

            if (portfolio.Visibility == CreatorVisibility.Public)
                return true;

            if (memberId != null && portfolio.CreatorProfile.MemberID == memberId)
                return true;

            if (portfolio.Visibility == CreatorVisibility.Private)
                return false;

            if (portfolio.Visibility == CreatorVisibility.Followers)
            {
                if (memberId == null)
                    return false;

                return await _context.FollowCreators
                    .AnyAsync(f =>
                        f.CreatorID == portfolio.CreatorID &&
                        f.MemberID == memberId);
            }

            return false;
        }

        public async Task<List<VMCreatorPortfolioListItem>> GetCreatorPortfoliosAsync(string creatorId)
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
                    CoverImageUrl = p.PortfolioItems
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    CreatorID = p.CreatorID
                })
                .ToListAsync();
        }

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

        public async Task CreateAsync(
            CreateCreatorPortfolioDTO dto,
            string creatorId,
            List<IFormFile> files)
        {
            var portfolioId = Guid.NewGuid().ToString();
            var now = DateTime.Now;

            var portfolio = new Portfolio
            {
                PortfolioID = portfolioId,
                Title = dto.Title,
                Description = dto.Description,
                Visibility = dto.Visibility,
                CreatorID = creatorId,
                StatusID = 1,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Portfolios.Add(portfolio);

            byte sort = 1;

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

                _context.PortfolioItems.Add(new PortfolioItem
                {
                    PortfolioID = portfolioId,
                    ImageUrl = imageKey,
                    SortOrder = sort++,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            await _context.SaveChangesAsync();

            if (portfolio.Visibility == CreatorVisibility.Public ||
                portfolio.Visibility == CreatorVisibility.Followers)
            {
                var followerMemberIds = await _context.FollowCreators
                    .Where(x => x.CreatorID == creatorId)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (followerMemberIds.Any())
                {
                    var dtos = followerMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.CreatorNewPortfolio,
                        Title = "創作者新作品集通知",
                        Content = $"你追蹤的創作者發布了新作品集「{portfolio.Title}」。",
                        LinkUrl = $"/Portfolio/Detail/{portfolio.PortfolioID}",
                        RelatedEntityType = "Portfolio",
                        RelatedEntityId = portfolio.PortfolioID
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }
        }

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

            portfolio.StatusID = 3;
            portfolio.UpdatedAt = now;

            foreach (var item in portfolio.PortfolioItems!)
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

            byte order = 1;

            foreach (var item in items)
            {
                item.SortOrder = order++;
            }
        }
    }
}