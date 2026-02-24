namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommunityContentAnalysis
    {
        public int PostsThisMonth { get; set; }
        public int PostsLastMonth { get; set; }

        public decimal MonthlyGrowthRate { get; set; }

        public List<VMPostMonthlyTrend> MonthlyTrend { get; set; } = new();
    }
}
