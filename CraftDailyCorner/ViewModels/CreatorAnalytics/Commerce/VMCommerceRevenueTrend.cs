namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce
{
    public class VMCommerceRevenueTrend
    {
        public decimal MonthlyGrowthRate { get; set; }
        public List<VMRevenueMonthlyTrend> MonthlyTrend { get; set; } = new();
    }

    public class VMRevenueMonthlyTrend
    {
        public string MonthLabel { get; set; } = null!;
        public decimal Revenue { get; set; }
    }
}
