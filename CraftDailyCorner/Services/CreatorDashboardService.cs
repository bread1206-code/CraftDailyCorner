using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorDashboardService : ICreatorDashboardService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorDashboardService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMCreatorDashboard?> GetDashboardAsync(string memberId)
        {
            //確認是否為創作者角色
            var isCreator = await _context.MemberRoles
                .AnyAsync(r => r.MemberID == memberId && r.RoleID == "02");

            if (!isCreator)
                return null;

            //取得 CreatorProfile
            var creator = await _context.CreatorProfiles
                .Where(c => c.MemberID == memberId)
                .Select(c => new
                {
                    c.CreatorID,
                    c.DisplayName,
                    c.ImageUrl,
                    c.Intro,
                    c.StartDate,
                    c.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (creator == null)
                return null;

            //統計資料（只計算正常狀態）
            var productCount = await _context.Products
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 2);//上架中

            var postCount = await _context.CreatorPosts
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 1);//啟用   

            var portfolioCount = await _context.Portfolios
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 1);//啟用


            // 回傳 Dashboard VM
            return new VMCreatorDashboard
            {
                CreatorID = creator.CreatorID,
                DisplayName = creator.DisplayName,
                ImageUrl = creator.ImageUrl,
                Intro = creator.Intro,
                StartDate = creator.StartDate,
                CreatedAt = creator.CreatedAt,

                ProductCount = productCount,
                PostCount = postCount,
                PortfolioCount = portfolioCount
            };
        }
    }
}