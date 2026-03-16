using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Community;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorAnalyticsService : ICreatorAnalyticsService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorAnalyticsService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 社群經營儀表板
        public async Task<VMCommunityDashboard> GetCommunityDashboardAsync(string creatorId)
        {
            var now = DateTime.Now;
            var currentYear = now.Year;
            var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);
            var firstDayThisYear = new DateTime(currentYear, 1, 1);
            var firstDayNextYear = firstDayThisYear.AddYears(1);

            var dashboard = new VMCommunityDashboard();

            // ===== Base Queries =====
            var postsQuery = _context.CreatorPosts
                .Where(p => p.CreatorID == creatorId);

            var portfoliosQuery = _context.Portfolios
                .Where(p => p.CreatorID == creatorId);

            var commentsQuery = _context.PostComments
                .Where(c => c.CreatorPost.CreatorID == creatorId);

            // ===== Overview (社群) =====
            var totalPosts = await postsQuery.CountAsync();
            var publishedPosts = await postsQuery.CountAsync(p => p.StatusID == 1);
            var draftPosts = totalPosts - publishedPosts;

            var totalPortfolios = await portfoliosQuery.CountAsync();
            var totalComments = await commentsQuery.CountAsync();

            var postsThisMonth = await postsQuery
                .Where(p => p.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            var portfoliosThisMonth = await portfoliosQuery
                .Where(p => p.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            var commentsThisMonth = await commentsQuery
                .Where(c => c.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            // ===== Reactions (總數 / 本月) =====
            var postReactionsQuery = _context.Reactions
                .Where(r => r.TargetType == ReactionTargetType.CreatorPost)
                .Join(
                    postsQuery,
                    r => r.TargetID,
                    p => p.PostID,
                    (r, p) => r
                );

            var portfolioReactionsQuery = _context.Reactions
                .Where(r => r.TargetType == ReactionTargetType.Portfolio)
                .Join(
                    portfoliosQuery,
                    r => r.TargetID,
                    p => p.PortfolioID,
                    (r, p) => r
                );

            var commentReactionsQuery = _context.Reactions
                .Where(r => r.TargetType == ReactionTargetType.PostComment)
                .Join(
                    commentsQuery,
                    r => r.TargetID,
                    c => c.CommentID,
                    (r, c) => r
                );

            var allReactionsQuery = postReactionsQuery
                .Concat(portfolioReactionsQuery)
                .Concat(commentReactionsQuery);

            var totalReactions = await allReactionsQuery.CountAsync();

            var reactionsThisMonth = await allReactionsQuery
                .Where(r => r.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            dashboard.Overview = new VMCommunityOverview
            {
                TotalPosts = totalPosts,
                PublishedPosts = publishedPosts,
                DraftPosts = draftPosts,
                TotalPortfolios = totalPortfolios,
                TotalComments = totalComments,
                TotalReactions = totalReactions,

                PostsThisMonth = postsThisMonth,
                PortfoliosThisMonth = portfoliosThisMonth,
                CommentsThisMonth = commentsThisMonth,
                ReactionsThisMonth = reactionsThisMonth
            };

            // ===== Content Analysis（發文趨勢 + 發文成長率）=====
            var postsLastMonth = await postsQuery
                .Where(p => p.CreatedAt >= firstDayLastMonth && p.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal postGrowth = 0;
            if (postsLastMonth > 0)
                postGrowth = (decimal)(postsThisMonth - postsLastMonth) / postsLastMonth;

            var postMonthlyRaw = await postsQuery
                .Where(p => p.CreatedAt >= firstDayThisYear && p.CreatedAt < firstDayNextYear)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    PostCount = g.Count()
                })
                .ToListAsync();

            dashboard.ContentAnalysis = new VMCommunityContentAnalysis
            {
                PostsThisMonth = postsThisMonth,
                PostsLastMonth = postsLastMonth,
                MonthlyGrowthRate = postGrowth,
                MonthlyTrend = Enumerable.Range(1, 12)
                    .Select(month => new VMPostMonthlyTrend
                    {
                        MonthLabel = $"{month}月",
                        PostCount = postMonthlyRaw
                            .FirstOrDefault(x => x.Month == month)?.PostCount ?? 0
                    })
                    .ToList()
            };

            // ===== Portfolio Analysis =====
            var portfoliosLastMonth = await portfoliosQuery
                .Where(p => p.CreatedAt >= firstDayLastMonth && p.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal portfolioGrowth = 0;
            if (portfoliosLastMonth > 0)
                portfolioGrowth = (decimal)(portfoliosThisMonth - portfoliosLastMonth) / portfoliosLastMonth;

            var portfolioMonthlyRaw = await portfoliosQuery
                .Where(p => p.CreatedAt >= firstDayThisYear && p.CreatedAt < firstDayNextYear)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    PortfolioCount = g.Count()
                })
                .ToListAsync();

            dashboard.PortfolioAnalysis = new VMCommunityPortfolioAnalysis
            {
                PortfoliosThisMonth = portfoliosThisMonth,
                PortfoliosLastMonth = portfoliosLastMonth,
                MonthlyGrowthRate = portfolioGrowth,
                MonthlyTrend = Enumerable.Range(1, 12)
                    .Select(month => new VMPortfolioMonthlyTrend
                    {
                        MonthLabel = $"{month}月",
                        PortfolioCount = portfolioMonthlyRaw
                            .FirstOrDefault(x => x.Month == month)?.PortfolioCount ?? 0
                    })
                    .ToList()
            };

            // ===== Interaction Analysis（留言趨勢 + Top留言貼文）=====
            var commentsLastMonth = await commentsQuery
                .Where(c => c.CreatedAt >= firstDayLastMonth && c.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal commentGrowth = 0;
            if (commentsLastMonth > 0)
                commentGrowth = (decimal)(commentsThisMonth - commentsLastMonth) / commentsLastMonth;

            var commentMonthlyRaw = await commentsQuery
                .Where(c => c.CreatedAt >= firstDayThisYear && c.CreatedAt < firstDayNextYear)
                .GroupBy(c => c.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    CommentCount = g.Count()
                })
                .ToListAsync();

            var topCommentPosts = await postsQuery
                .Select(p => new VMPostCommentRanking
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    CommentCount = _context.PostComments.Count(c => c.PostID == p.PostID),
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(x => x.CommentCount)
                .Take(5)
                .ToListAsync();

            dashboard.InteractionAnalysis = new VMCommunityInteractionAnalysis
            {
                CommentsThisMonth = commentsThisMonth,
                CommentsLastMonth = commentsLastMonth,
                CommentGrowthRate = commentGrowth,
                CommentTrend = Enumerable.Range(1, 12)
                    .Select(month => new VMCommentMonthlyTrend
                    {
                        MonthLabel = $"{month}月",
                        CommentCount = commentMonthlyRaw
                            .FirstOrDefault(x => x.Month == month)?.CommentCount ?? 0
                    })
                    .ToList(),
                TopCommentPosts = topCommentPosts
            };

            // ===== Reaction Analysis（趨勢 + 成長率 + Top內容）=====
            var reactionsLastMonth = await allReactionsQuery
                .Where(r => r.CreatedAt >= firstDayLastMonth && r.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal reactionGrowth = 0;
            if (reactionsLastMonth > 0)
                reactionGrowth = (decimal)(reactionsThisMonth - reactionsLastMonth) / reactionsLastMonth;

            var reactionMonthlyRaw = await allReactionsQuery
                .Where(r => r.CreatedAt >= firstDayThisYear && r.CreatedAt < firstDayNextYear)
                .GroupBy(r => r.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    ReactionCount = g.Count()
                })
                .ToListAsync();

            var topReactedPosts = await postReactionsQuery
                .GroupBy(r => r.TargetID)
                .Select(g => new
                {
                    PostID = g.Key,
                    ReactionCount = g.Count()
                })
                .OrderByDescending(x => x.ReactionCount)
                .Take(5)
                .Join(
                    postsQuery,
                    x => x.PostID,
                    p => p.PostID,
                    (x, p) => new VMPostReactionRanking
                    {
                        PostID = p.PostID,
                        Title = p.Title,
                        ReactionCount = x.ReactionCount,
                        CreatedAt = p.CreatedAt
                    }
                )
                .ToListAsync();

            var topReactedPortfolios = await portfolioReactionsQuery
                .GroupBy(r => r.TargetID)
                .Select(g => new
                {
                    PortfolioID = g.Key,
                    ReactionCount = g.Count()
                })
                .OrderByDescending(x => x.ReactionCount)
                .Take(5)
                .Join(
                    portfoliosQuery,
                    x => x.PortfolioID,
                    p => p.PortfolioID,
                    (x, p) => new VMPortfolioReactionRanking
                    {
                        PortfolioID = p.PortfolioID,
                        Title = p.Title,
                        ReactionCount = x.ReactionCount,
                        CreatedAt = p.CreatedAt
                    }
                )
                .ToListAsync();

            dashboard.ReactionAnalysis = new VMCommunityReactionAnalysis
            {
                ReactionsThisMonth = reactionsThisMonth,
                ReactionsLastMonth = reactionsLastMonth,
                ReactionGrowthRate = reactionGrowth,
                MonthlyTrend = Enumerable.Range(1, 12)
                    .Select(month => new VMReactionMonthlyTrend
                    {
                        MonthLabel = $"{month}月",
                        ReactionCount = reactionMonthlyRaw
                            .FirstOrDefault(x => x.Month == month)?.ReactionCount ?? 0
                    })
                    .ToList(),
                TopReactedPosts = topReactedPosts,
                TopReactedPortfolios = topReactedPortfolios
            };

            return dashboard;
        }

        // 電商銷售儀表板
        public async Task<VMCommerceDashboard> GetCommerceDashboardAsync(string creatorId)
        {
            var now = DateTime.Now;
            var currentYear = now.Year;
            var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);
            var firstDayThisYear = new DateTime(currentYear, 1, 1);
            var firstDayNextYear = firstDayThisYear.AddYears(1);

            var dashboard = new VMCommerceDashboard();

            var orderQuery = _context.OrderDetails
                .Where(od => od.Product.CreatorID == creatorId);

            // ===== 本月 / 上月 KPI =====
            var thisMonthQuery = orderQuery
                .Where(od => od.Order.CreatedAt >= firstDayThisMonth);

            var lastMonthQuery = orderQuery
                .Where(od => od.Order.CreatedAt >= firstDayLastMonth &&
                             od.Order.CreatedAt < firstDayThisMonth);

            var thisMonthOrders = await thisMonthQuery
                .Select(x => x.OrderID)
                .Distinct()
                .CountAsync();

            var lastMonthOrders = await lastMonthQuery
                .Select(x => x.OrderID)
                .Distinct()
                .CountAsync();

            var thisMonthQty = await thisMonthQuery.SumAsync(x => (int?)x.Quantity) ?? 0;
            var lastMonthQty = await lastMonthQuery.SumAsync(x => (int?)x.Quantity) ?? 0;

            var thisMonthRevenue = await thisMonthQuery
                .SumAsync(x => (decimal?)x.Quantity * x.PriceSnapshot) ?? 0;

            var lastMonthRevenue = await lastMonthQuery
                .SumAsync(x => (decimal?)x.Quantity * x.PriceSnapshot) ?? 0;

            var aov = thisMonthOrders > 0 ? thisMonthRevenue / thisMonthOrders : 0;

            dashboard.Overview = new VMCommerceOverview
            {
                Revenue = new VMCommerceKpiDelta<decimal>
                {
                    Current = thisMonthRevenue,
                    Previous = lastMonthRevenue,
                    GrowthRate = CalculateGrowth(thisMonthRevenue, lastMonthRevenue)
                },
                Orders = new VMCommerceKpiDelta<int>
                {
                    Current = thisMonthOrders,
                    Previous = lastMonthOrders,
                    GrowthRate = CalculateGrowth(thisMonthOrders, lastMonthOrders)
                },
                Quantity = new VMCommerceKpiDelta<int>
                {
                    Current = thisMonthQty,
                    Previous = lastMonthQty,
                    GrowthRate = CalculateGrowth(thisMonthQty, lastMonthQty)
                },
                AverageOrderValue = aov
            };

            // ===== 月營收趨勢（當年 1~12 月，缺值補 0）=====
            var revenueRaw = await orderQuery
                .Where(x => x.Order.CreatedAt >= firstDayThisYear &&
                            x.Order.CreatedAt < firstDayNextYear)
                .GroupBy(x => x.Order.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.Quantity * x.PriceSnapshot)
                })
                .ToListAsync();

            var revenueTrend = Enumerable.Range(1, 12)
                .Select(month => new VMRevenueMonthlyTrend
                {
                    MonthLabel = $"{month}月",
                    Revenue = revenueRaw
                        .FirstOrDefault(x => x.Month == month)?.Revenue ?? 0
                })
                .ToList();

            dashboard.RevenueTrend = new VMCommerceRevenueTrend
            {
                MonthlyTrend = revenueTrend,
                MonthlyGrowthRate = CalculateGrowth(thisMonthRevenue, lastMonthRevenue)
            };

            // ===== 月訂單趨勢（當年 1~12 月，缺值補 0）=====
            var orderRaw = await orderQuery
                .Where(x => x.Order.CreatedAt >= firstDayThisYear &&
                            x.Order.CreatedAt < firstDayNextYear)
                .GroupBy(x => x.Order.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    OrderCount = g.Select(x => x.OrderID).Distinct().Count()
                })
                .ToListAsync();

            dashboard.OrderTrend = new VMCommerceOrderTrend
            {
                MonthlyTrend = Enumerable.Range(1, 12)
                    .Select(month => new VMOrderMonthlyTrend
                    {
                        MonthLabel = $"{month}月",
                        OrderCount = orderRaw
                            .FirstOrDefault(x => x.Month == month)?.OrderCount ?? 0
                    })
                    .ToList()
            };

            // ===== 商品排行 =====
            var topByRevenue = await orderQuery
                .GroupBy(x => new { x.ProductID, x.Product.ProductName })
                .Select(g => new VMProductSalesRanking
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.PriceSnapshot)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToListAsync();

            var topByQuantity = await orderQuery
                .GroupBy(x => new { x.ProductID, x.Product.ProductName })
                .Select(g => new VMProductSalesRanking
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.PriceSnapshot)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToListAsync();

            dashboard.ProductRanking = new VMCommerceProductRanking
            {
                TopByRevenue = topByRevenue,
                TopByQuantity = topByQuantity
            };

            return dashboard;
        }

        private decimal CalculateGrowth(decimal current, decimal previous)
        {
            if (previous == 0)
                return current > 0 ? 1 : 0;

            return (current - previous) / previous;
        }

        private decimal CalculateGrowth(int current, int previous)
        {
            if (previous == 0)
                return current > 0 ? 1 : 0;

            return (decimal)(current - previous) / previous;
        }
    }
}