using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class FollowService: IFollowService
    {
        private readonly CraftDailyCornerContext _context;

        public FollowService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task ToggleAsync(string creatorId, string memberId, string? loginCreatorId)
        {
            // 🚫 創作者本人不能追蹤自己
            if (!string.IsNullOrEmpty(loginCreatorId) && loginCreatorId == creatorId)
                throw new InvalidOperationException("不能追蹤自己");

            var existing = await _context.FollowCreators
                .FirstOrDefaultAsync(f =>
                    f.CreatorID == creatorId &&
                    f.MemberID == memberId);

            if (existing == null)
            {
                _context.FollowCreators.Add(new FollowCreator
                {
                    CreatorID = creatorId,
                    MemberID = memberId,
                    CreatedAt = DateTime.Now
                });
            }
            else
            {
                _context.FollowCreators.Remove(existing);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowingAsync(string creatorId, string memberId)
        {
            return await _context.FollowCreators
                .AnyAsync(f =>
                    f.CreatorID == creatorId &&
                    f.MemberID == memberId);
        }

        public async Task<int> GetFollowerCountAsync(string creatorId)
        {
            return await _context.FollowCreators
                .CountAsync(f => f.CreatorID == creatorId);
        }

        // 取得會員追蹤的創作者列表
        public async Task<List<VMFollowingCreatorCard>> GetMyFollowingAsync(string memberId)
        {
            return await _context.FollowCreators
                .Where(f => f.MemberID == memberId)
                .Select(f => new VMFollowingCreatorCard
                {
                    //創作者基本資訊
                    CreatorId = f.CreatorID,
                    CreatorName = f.CreatorProfile.DisplayName,
                    CreatorLogo = f.CreatorProfile.ImageUrl,

                    //最新商品
                    LatestProductId = _context.Products
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 2)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.ProductID)
                        .FirstOrDefault(),

                    LatestProductName = _context.Products
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 2)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.ProductName)
                        .FirstOrDefault(),

                    LatestProductImage = _context.ProductImages
                        .Where(i =>
                            i.Product.CreatorID == f.CreatorID &&
                            i.Product.StatusID == 2 &&
                            i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),

                    //最新日誌
                    LatestPostId = _context.CreatorPosts
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 1)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.PostID)
                        .FirstOrDefault(),

                    LatestPostTitle = _context.CreatorPosts
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 1)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.Title)
                        .FirstOrDefault(),

                    LatestPostImage = _context.CreatorPosts
                        .Where(i =>
                            i.CreatorID == f.CreatorID &&i.StatusID == 1)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),

                    //最新作品集
                    LatestPortfolioId = _context.Portfolios
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 1)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.PortfolioID)
                        .FirstOrDefault(),

                    LatestPortfolioTitle = _context.Portfolios
                        .Where(p => p.CreatorID == f.CreatorID && p.StatusID == 1)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.Title)
                        .FirstOrDefault(),

                    LatestPortfolioImage = _context.PortfolioItems
                        .Where(i =>
                            i.Portfolio.CreatorID == f.CreatorID &&i.Portfolio.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }
    }
}
