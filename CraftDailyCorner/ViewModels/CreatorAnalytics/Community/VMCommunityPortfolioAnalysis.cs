namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMCommunityPortfolioAnalysis
    {
        public int PortfoliosThisMonth { get; set; }
        public int PortfoliosLastMonth { get; set; }
        public decimal MonthlyGrowthRate { get; set; }

        public List<VMPortfolioMonthlyTrend> MonthlyTrend { get; set; } = new();
    }
}