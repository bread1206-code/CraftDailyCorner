using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.CreatorPost.Front;
using CraftDailyCorner.ViewModels.FollowCreator;
using CraftDailyCorner.ViewModels.Product;
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
            string? memberId)
        {
            var creator = await _context.CreatorProfiles
                .Where(c => c.CreatorID == creatorId)
                .Select(c => new
                {
                    c.CreatorID,
                    c.DisplayName,
                    c.ImageUrl,
                    c.Intro,
                    c.StartDate,

                    LatestPosts = c.CreatorPosts
                        .Where(p => p.StatusID == 1 &&
                                    p.Visibility == CreatorPostVisibility.Public)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(6)
                        .Select(p => new VMCreatorPostPublicListItem
                        {
                            PostID = p.PostID,
                            Title = p.Title,
                            ImageUrl = p.ImageUrl,
                            CreatedAt = p.CreatedAt,
                            CreatorName = c.DisplayName
                        }).ToList(),

                    LatestPortfolios = c.Portfolios
                        .Where(p => p.StatusID == 1 &&
                                    p.Visibility == CreatorPostVisibility.Public)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(6)
                        .Select(p => new VMCreatorPortfolioPublicListItem
                        {
                            PortfolioID = p.PortfolioID,
                            Title = p.Title,
                            CreatedAt = p.CreatedAt,
                            CreatorName = c.DisplayName,
                            ItemCount = p.PortfolioItems.Count()
                        }).ToList(),
                    LatestProducts = c.Products
                        .Where(p => p.StatusID == 1)// 上架中
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(6)
                        .Select(p => new VMCreatorProductPublicListItem
                        {
                            ProductID = p.ProductID,
                            ProductName = p.ProductName,
                            ImageUrl = p.ProductImages.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl).FirstOrDefault(),
                            Price = p.Price,
                            CreatedAt = p.CreatedAt
                        }).ToList()
                                    })
                .FirstOrDefaultAsync();

            if (creator == null)
                return null;

            // ===== Follow 狀態 =====
            var followerCount =
                await _followService.GetFollowerCountAsync(creatorId);

            var isFollowing = false;

            if (!string.IsNullOrEmpty(memberId))
            {
                isFollowing = await _followService
                    .IsFollowingAsync(creatorId, memberId);
            }

            return new VMCreatorPublicProfile
            {
                CreatorID = creator.CreatorID,
                DisplayName = creator.DisplayName,
                ImageUrl = creator.ImageUrl,
                Intro = creator.Intro,
                StartDate = creator.StartDate,
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
        }
        public async Task<VMCreatorIndex> GetCreatorIndexAsync()
        {
            var creators = await _context.CreatorProfiles
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new VMCreatorIndexItem
                {
                    CreatorID = c.CreatorID,
                    DisplayName = c.DisplayName,
                    ImageUrl = c.ImageUrl,
                    Intro = c.Intro,
                    CreatedAt = c.CreatedAt,
                    FollowerCount = _context.FollowCreators
                        .Count(f => f.CreatorID == c.CreatorID)
                })
                .ToListAsync();

            return new VMCreatorIndex
            {
                Creators = creators
            };
        }
    }
}