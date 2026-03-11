using CraftDailyCorner.Areas.Admin.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CraftDailyCorner.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IMemoryCache _memoryCache;

        public AdminDashboardService(
            CraftDailyCornerContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // =========================
        // ① 主 Dashboard KPI
        // =========================

        public async Task<VMDashboard> GetDashboardAsync()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            var todayOrders = await _context.Orders
                .Where(o => o.CreatedAt >= today)
                .CountAsync();

            var yesterdayOrders = await _context.Orders
                .Where(o => o.CreatedAt >= yesterday && o.CreatedAt < today)
                .CountAsync();

            var todayRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= today)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var yesterdayRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= yesterday && o.CreatedAt < today)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var todayMembers = await _context.Members
                .Where(m => m.CreatedAt >= today)
                .CountAsync();

            var yesterdayMembers = await _context.Members
                .Where(m => m.CreatedAt >= yesterday && m.CreatedAt < today)
                .CountAsync();

            return new VMDashboard
            {
                TodayOrders = todayOrders,
                YesterdayOrders = yesterdayOrders,
                TodayRevenue = todayRevenue,
                TodayMembers = todayMembers,

                OrderGrowthRate = CalculateGrowth(todayOrders, yesterdayOrders),
                RevenueGrowthRate = CalculateGrowth(todayRevenue, yesterdayRevenue),
                MemberGrowthRate = CalculateGrowth(todayMembers, yesterdayMembers),

                OrderGrowthUp = todayOrders > yesterdayOrders,
                RevenueGrowthUp = todayRevenue > yesterdayRevenue,
                MemberGrowthUp = todayMembers > yesterdayMembers,

                AvailableMonths = await GenerateAvailableMonths()
            };
        }

        // =========================
        // ② 區間圖表資料
        // =========================

        public async Task<object> GetChartDataAsync(string range)
        {
            var cacheKey = $"Dashboard_{range}";

            if (_memoryCache.TryGetValue(cacheKey, out object cached))
                return cached;

            var days = range switch
            {
                "7" => 7,
                "30" => 30,
                _ => DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)
            };

            var startDate = DateTime.Today.AddDays(-(days - 1));

            var dateRange = Enumerable.Range(0, days)
                .Select(i => startDate.AddDays(i))
                .ToList();

            var orders = await _context.Orders
                .Where(o => o.CreatedAt >= startDate)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            var members = await _context.Members
                .Where(m => m.CreatedAt >= startDate)
                .GroupBy(m => m.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var result = new
            {
                labels = dateRange.Select(d => d.ToString("MM/dd")),
                orderData = dateRange.Select(d => orders.FirstOrDefault(o => o.Date == d)?.Count ?? 0),
                revenueData = dateRange.Select(d => orders.FirstOrDefault(o => o.Date == d)?.Revenue ?? 0),
                memberData = dateRange.Select(d => members.FirstOrDefault(m => m.Date == d)?.Count ?? 0)
            };

            _memoryCache.Set(cacheKey, result, GetCacheDuration(range));

            return result;
        }

        // =========================
        // ③ 歷史月份圖表
        // =========================

        public async Task<object> GetHistoryMonthDataAsync(string month)
        {
            var cacheKey = $"History_{month}";

            if (_memoryCache.TryGetValue(cacheKey, out object cached))
                return cached;

            var date = DateTime.Parse($"{month}-01");
            var endDate = date.AddMonths(1);

            var orders = await _context.Orders
                .Where(o => o.CreatedAt >= date && o.CreatedAt < endDate)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            var days = DateTime.DaysInMonth(date.Year, date.Month);

            var dateRange = Enumerable.Range(0, days)
                .Select(i => date.AddDays(i))
                .ToList();

            var result = new
            {
                labels = dateRange.Select(d => d.ToString("MM/dd")),
                orderData = dateRange.Select(d => orders.FirstOrDefault(o => o.Date == d)?.Count ?? 0),
                revenueData = dateRange.Select(d => orders.FirstOrDefault(o => o.Date == d)?.Revenue ?? 0)
            };

            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }

        // =========================
        // 工具方法
        // =========================

        private decimal CalculateGrowth(decimal today, decimal yesterday)
        {
            if (yesterday == 0)
                return today > 0 ? 100m : 0m;

            return Math.Round(((today - yesterday) / yesterday) * 100m, 2);
        }

        private TimeSpan GetCacheDuration(string range)
        {
            return range switch
            {
                "7" => TimeSpan.FromMinutes(5),
                "30" => TimeSpan.FromMinutes(10),
                _ => TimeSpan.FromMinutes(5)
            };
        }

        private async Task<List<string>> GenerateAvailableMonths()
        {
            var oldest = await _context.Orders
                .OrderBy(o => o.CreatedAt)
                .Select(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (oldest == default)
                return new List<string>();

            var firstMonth = new DateTime(oldest.Year, oldest.Month, 1);
            var lastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                .AddMonths(-1);

            var list = new List<string>();

            while (firstMonth <= lastMonth)
            {
                list.Add(firstMonth.ToString("yyyy-MM"));
                firstMonth = firstMonth.AddMonths(1);
            }

            list.Reverse(); // 最新在前

            return list;
        }
    }
}