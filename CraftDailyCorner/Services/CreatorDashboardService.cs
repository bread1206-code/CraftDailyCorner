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
                    c.BrandName,
                    c.ImageUrl,
                    c.BrandIntro,
                    c.StartDate,
                    c.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (creator == null)
                return null;

            //基本統計資料
            var productCount = await _context.Products
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 2); // 上架中

            var inventoryAlertCount = await _context.Products
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 2 &&
                    p.Inventory.StockQty <= p.Inventory.AlertQty);

            var postCount = await _context.CreatorPosts
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 1); // 啟用

            var portfolioCount = await _context.Portfolios
                .CountAsync(p =>
                    p.CreatorID == creator.CreatorID &&
                    p.StatusID == 1); // 啟用


            //訂單統計
            var orderGrouped = await _context.Orders
                .Where(o => o.OrderDetails
                    .Any(d => d.Product.CreatorID == creator.CreatorID))
                .GroupBy(o => o.StatusID)
                .Select(g => new
                {
                    StatusID = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            int newCount = orderGrouped
                .FirstOrDefault(x => x.StatusID == 2)?.Count ?? 0; // Paid

            int processingCount = orderGrouped
                .FirstOrDefault(x => x.StatusID == 3)?.Count ?? 0;

            int shippingCount = orderGrouped
                .FirstOrDefault(x => x.StatusID == 4)?.Count ?? 0;

            int historyCount = orderGrouped
                .Where(x => x.StatusID == 5 || x.StatusID == 6)
                .Sum(x => x.Count);

            //回傳 Dashboard VM
            return new VMCreatorDashboard
            {
                CreatorID = creator.CreatorID,
                BrandName = creator.BrandName,
                ImageUrl = creator.ImageUrl,
                BrandIntro = creator.BrandIntro,
                StartDate = creator.StartDate,
                CreatedAt = creator.CreatedAt,

                ProductCount = productCount,
                InventoryAlertCount = inventoryAlertCount,
                PostCount = postCount,
                PortfolioCount = portfolioCount,

                NewOrderCount = newCount,
                ProcessingOrderCount = processingCount,
                ShippingOrderCount = shippingCount,
                HistoryOrderCount = historyCount
            };
        }
    }
}