using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorAnalytics;
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

        //社群經營儀表板
        public async Task<VMCommunityDashboard> GetCommunityDashboardAsync(string creatorId)
        {
            var now = DateTime.Now;
            var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);

            var dashboard = new VMCommunityDashboard();

            //Overview

            var postsQuery = _context.CreatorPosts
                .Where(p => p.CreatorID == creatorId);

            var totalPosts = await postsQuery.CountAsync();
            var publishedPosts = await postsQuery.CountAsync(p => p.StatusID == 1);
            var draftPosts = totalPosts - publishedPosts;

            var totalComments = await _context.PostComments
                .Where(c => c.CreatorPost.CreatorID == creatorId)
                .CountAsync();

            var totalProducts = await _context.Products
                .Where(p => p.CreatorID == creatorId)
                .CountAsync();

            var orderItemsQuery = _context.OrderDetails
                .Where(o => o.Product.CreatorID == creatorId);

            var totalOrders = await orderItemsQuery
                .Select(o => o.OrderID)
                .Distinct()
                .CountAsync();

            var totalRevenue = await orderItemsQuery
                .SumAsync(o => (decimal?)o.Quantity * o.Product.Price) ?? 0;

            dashboard.Overview = new VMCommunityOverview
            {
                TotalPosts = totalPosts,
                PublishedPosts = publishedPosts,
                DraftPosts = draftPosts,
                TotalComments = totalComments,
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue
            };

            //Content Analysis

            var postsThisMonth = await postsQuery
                .Where(p => p.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            var postsLastMonth = await postsQuery
                .Where(p => p.CreatedAt >= firstDayLastMonth &&
                            p.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal monthlyGrowth = 0;
            if (postsLastMonth > 0)
                monthlyGrowth = (decimal)(postsThisMonth - postsLastMonth) / postsLastMonth;

            var monthlyRaw = await postsQuery
                .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    PostCount = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var monthlyTrend = monthlyRaw
                .Select(x => new VMPostMonthlyTrend
                {
                    MonthLabel = $"{x.Year}-{x.Month:D2}",
                    PostCount = x.PostCount
                })
                .ToList();

            dashboard.ContentAnalysis = new VMCommunityContentAnalysis
            {
                PostsThisMonth = postsThisMonth,
                PostsLastMonth = postsLastMonth,
                MonthlyGrowthRate = monthlyGrowth,
                MonthlyTrend = monthlyTrend
            };

            //Interaction Analysis

            var commentsThisMonth = await _context.PostComments
                .Where(c => c.CreatorPost.CreatorID == creatorId &&
                            c.CreatedAt >= firstDayThisMonth)
                .CountAsync();

            var commentsLastMonth = await _context.PostComments
                .Where(c => c.CreatorPost.CreatorID == creatorId &&
                            c.CreatedAt >= firstDayLastMonth &&
                            c.CreatedAt < firstDayThisMonth)
                .CountAsync();

            decimal commentGrowth = 0;
            if (commentsLastMonth > 0)
                commentGrowth = (decimal)(commentsThisMonth - commentsLastMonth) / commentsLastMonth;

            var topPosts = await postsQuery
                .Select(p => new VMPostCommentRanking
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    CommentCount = p.PostComments.Count(),
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.CommentCount)
                .Take(5)
                .ToListAsync();

            var commentRaw = await _context.PostComments
                .Where(c => c.CreatorPost.CreatorID == creatorId)
                .GroupBy(c => new { c.CreatedAt.Year, c.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    CommentCount = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var commentTrend = commentRaw
                .Select(x => new VMCommentMonthlyTrend
                {
                    MonthLabel = $"{x.Year}-{x.Month:D2}",
                    CommentCount = x.CommentCount
                })
                .ToList();

            dashboard.InteractionAnalysis = new VMCommunityInteractionAnalysis
            {
                CommentsThisMonth = commentsThisMonth,
                CommentsLastMonth = commentsLastMonth,
                CommentGrowthRate = commentGrowth,
                TopCommentPosts = topPosts,
                CommentTrend = commentTrend
            };

            //Business Analysis

            var topProducts = await orderItemsQuery
                .GroupBy(o => new { o.ProductID, o.Product.ProductName })
                .Select(g => new VMProductSalesRanking
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.Product.Price)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(5)
                .ToListAsync();

            decimal avgOrderValue = 0;
            if (totalOrders > 0)
                avgOrderValue = totalRevenue / totalOrders;

            dashboard.BusinessAnalysis = new VMCommunityBusinessAnalysis
            {
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                AverageOrderValue = avgOrderValue,
                TopSellingProducts = topProducts
            };

            return dashboard;
        }
        //電商銷售儀表板
        public async Task<VMCommerceDashboard> GetCommerceDashboardAsync(string creatorId)
        {
            var now = DateTime.Now;
            var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);

            var dashboard = new VMCommerceDashboard();

            var orderQuery = _context.OrderDetails
                .Where(o => o.Product.CreatorID == creatorId);

            //Overview

            var totalOrders = await orderQuery
                .Select(o => o.OrderID)
                .Distinct()
                .CountAsync();

            var totalQuantity = await orderQuery
                .SumAsync(o => (int?)o.Quantity) ?? 0;

            var totalRevenue = await orderQuery
                .SumAsync(o => (decimal?)o.Quantity * o.Product.Price) ?? 0;

            decimal avgOrderValue = 0;
            if (totalOrders > 0)
                avgOrderValue = totalRevenue / totalOrders;

            dashboard.Overview = new VMCommerceOverview
            {
                TotalOrders = totalOrders,
                TotalQuantitySold = totalQuantity,
                TotalRevenue = totalRevenue,
                AverageOrderValue = avgOrderValue
            };

            //Revenue Trend

            var revenueRaw = await orderQuery
                .GroupBy(o => new { o.Order.CreatedAt.Year, o.Order.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Revenue = g.Sum(x => x.Quantity * x.Product.Price)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var monthlyTrend = revenueRaw
                .Select(x => new VMRevenueMonthlyTrend
                {
                    MonthLabel = $"{x.Year}-{x.Month:D2}",
                    Revenue = x.Revenue
                })
                .ToList();

            var thisMonthRevenue = monthlyTrend
                .Where(x => x.MonthLabel == $"{now.Year}-{now.Month:D2}")
                .Select(x => x.Revenue)
                .FirstOrDefault();

            var lastMonthRevenue = monthlyTrend
                .Where(x => x.MonthLabel == $"{firstDayLastMonth.Year}-{firstDayLastMonth.Month:D2}")
                .Select(x => x.Revenue)
                .FirstOrDefault();

            decimal growth = 0;
            if (lastMonthRevenue > 0)
                growth = (thisMonthRevenue - lastMonthRevenue) / lastMonthRevenue;

            dashboard.RevenueTrend = new VMCommerceRevenueTrend
            {
                MonthlyTrend = monthlyTrend,
                MonthlyGrowthRate = growth
            };

            //Product Ranking

            var topByRevenue = await orderQuery
                .GroupBy(o => new { o.ProductID, o.Product.ProductName })
                .Select(g => new VMProductSalesRanking
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.Product.Price)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToListAsync();

            var topByQuantity = await orderQuery
                .GroupBy(o => new { o.ProductID, o.Product.ProductName })
                .Select(g => new VMProductSalesRanking
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.Product.Price)
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
    }
}