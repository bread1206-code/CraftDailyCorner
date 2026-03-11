using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.CreatorPost;
using CraftDailyCorner.ViewModels.FollowCreator;
using CraftDailyCorner.ViewModels.Product;
using CraftDailyCorner.ViewModels.Reaction;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPublicService : ICreatorPublicService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IFollowService _followService;

        public CreatorPublicService(
            CraftDailyCornerContext context,
            IFollowService followService)
        {
            _context = context;
            _followService = followService;
        }

        public async Task<VMCreatorPublicProfile?> GetProfileAsync(
            string creatorId,
            string? memberId,
            string? loginCreatorId)
        {
            var creator = await _context.CreatorProfiles
                .Where(c => c.CreatorID == creatorId)
                .Select(c => new
                {
                    c.CreatorID,
                    c.BrandName,
                    c.ImageUrl,
                    c.BrandIntro,
                    c.StartDate,

                    // ================= POSTS =================
                    LatestPosts = c.CreatorPosts
                        .Where(p => p.StatusID == 1 &&
                                    p.Visibility == CreatorPostVisibility.Public)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(3)
                        .Select(p => new VMPostListItem
                        {
                            PostID = p.PostID,
                            Title = p.Title,
                            ImageUrl = p.ImageUrl,
                            CreatedAt = p.CreatedAt,
                            BrandName = c.BrandName,

                            CommentCount = 0,
                            Preview = "",

                            ReactionSummary = new VMReactionButton
                            {
                                TargetType = ReactionTargetType.CreatorPost,
                                TargetID = p.PostID,

                                TotalCount = _context.Reactions
                                    .Count(r => r.TargetType == ReactionTargetType.CreatorPost
                                             && r.TargetID == p.PostID),

                                TopReactionType = _context.Reactions
                                    .Where(r => r.TargetType == ReactionTargetType.CreatorPost
                                             && r.TargetID == p.PostID)
                                    .GroupBy(r => r.ReactionType)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => (ReactionType?)g.Key)
                                    .FirstOrDefault(),

                                UserReactionType = memberId == null
                                    ? null
                                    : _context.Reactions
                                        .Where(r => r.TargetType == ReactionTargetType.CreatorPost
                                                 && r.TargetID == p.PostID
                                                 && r.MemberID == memberId)
                                        .Select(r => (ReactionType?)r.ReactionType)
                                        .FirstOrDefault()
                            }
                        })
                        .ToList(),

                    // ================= PORTFOLIOS =================
                    LatestPortfolios = c.Portfolios
                        .Where(p => p.StatusID == 1 &&
                                    p.Visibility == CreatorPostVisibility.Public)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(3)
                        .Select(p => new VMCreatorPortfolioPublicListItem
                        {
                            PortfolioID = p.PortfolioID,
                            Title = p.Title,
                            CreatedAt = p.CreatedAt,
                            CreatorName = c.BrandName,
                            ItemCount = p.PortfolioItems.Count(),
                            CoverImageUrl = p.PortfolioItems
                                .OrderBy(i => i.SortOrder)
                                .Select(i => i.ImageUrl)
                                .FirstOrDefault(),

                            Preview = "",

                            ReactionSummary = new VMReactionButton
                            {
                                TargetType = ReactionTargetType.Portfolio,
                                TargetID = p.PortfolioID,

                                TotalCount = _context.Reactions
                                    .Count(r => r.TargetType == ReactionTargetType.Portfolio
                                             && r.TargetID == p.PortfolioID),

                                TopReactionType = _context.Reactions
                                    .Where(r => r.TargetType == ReactionTargetType.Portfolio
                                             && r.TargetID == p.PortfolioID)
                                    .GroupBy(r => r.ReactionType)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => (ReactionType?)g.Key)
                                    .FirstOrDefault(),

                                UserReactionType = memberId == null
                                    ? null
                                    : _context.Reactions
                                        .Where(r => r.TargetType == ReactionTargetType.Portfolio
                                                 && r.TargetID == p.PortfolioID
                                                 && r.MemberID == memberId)
                                        .Select(r => (ReactionType?)r.ReactionType)
                                        .FirstOrDefault()
                            }
                        })
                        .ToList(),

                    // ================= PRODUCTS =================
                    LatestProducts = c.Products
                        .Where(p => p.StatusID == 2)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(3)
                        .Select(p => new VMCreatorProductPublicListItem
                        {
                            ProductID = p.ProductID,
                            ProductName = p.ProductName,
                            ImageUrl = p.ProductImages
                                .OrderBy(i => i.SortOrder)
                                .Select(i => i.ImageUrl)
                                .FirstOrDefault(),
                            Price = p.Price,
                            CreatedAt = p.CreatedAt,

                            IsFavorite = memberId == null
                                ? false
                                : _context.FavoriteProducts.Any(fp =>
                                    fp.MemberID == memberId &&
                                    fp.ProductID == p.ProductID)
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (creator == null)
                return null;
            var isOwner = !string.IsNullOrEmpty(loginCreatorId)
                  && loginCreatorId == creatorId;

            var followerCount =
                await _followService.GetFollowerCountAsync(creatorId);

            var isFollowing = false;

            // ✅ 本人不需要查追蹤狀態
            if (!isOwner && !string.IsNullOrEmpty(memberId))
            {
                isFollowing = await _followService
                    .IsFollowingAsync(creatorId, memberId);
            }

            var vm= new VMCreatorPublicProfile
            {
                CreatorID = creator.CreatorID,
                BrandName = creator.BrandName,
                ImageUrl = creator.ImageUrl,
                BrandIntro = creator.BrandIntro,
                StartDate = creator.StartDate,

                IsOwner = isOwner,

                LatestPosts = creator.LatestPosts,
                LatestPortfolios = creator.LatestPortfolios,
                LatestProducts = creator.LatestProducts,

                FollowInfo = new VMFollowButton
                {
                    CreatorID = creatorId,
                    IsFollowing = isFollowing,
                    FollowerCount = followerCount
                }
            };
            vm.FollowInfo.LogoUrl = vm.CreatorImagePath;
            return vm;
        }
        public async Task<VMCreatorIndex> GetCreatorIndexAsync(string? keyword, int page)
        {
            keyword = keyword?.Trim();
            if (page < 1) page = 1;

            var query = _context.CreatorProfiles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c =>
                    c.BrandName.Contains(keyword) ||
                    c.BrandIntro.Contains(keyword));
            }

            // PageSize 建議在 VM 預設 9；這裡也可以固定
            const int pageSize = 9;

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page > totalPages) page = totalPages;

            var creators = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new VMCreatorIndexItem
                {
                    CreatorID = c.CreatorID,
                    BrandName = c.BrandName,
                    ImageUrl = c.ImageUrl,
                    BrandIntro = c.BrandIntro,
                    CreatedAt = c.CreatedAt,
                    FollowerCount = _context.FollowCreators.Count(f => f.CreatorID == c.CreatorID)
                })
                .ToListAsync();

            return new VMCreatorIndex
            {
                Query = new VMCreatorIndexQuery
                {
                    Keyword = keyword,
                    Page = page,
                    PageSize = pageSize
                },
                Creators = creators,
                TotalPages = totalPages
            };
        }
    }
}