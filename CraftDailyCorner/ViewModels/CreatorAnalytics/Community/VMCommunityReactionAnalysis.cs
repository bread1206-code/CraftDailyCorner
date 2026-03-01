namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMCommunityReactionAnalysis
    {
        public int ReactionsThisMonth { get; set; }
        public int ReactionsLastMonth { get; set; }
        public decimal ReactionGrowthRate { get; set; }

        public List<VMReactionMonthlyTrend> MonthlyTrend { get; set; } = new();

        public List<VMPostReactionRanking> TopReactedPosts { get; set; } = new();
        public List<VMPortfolioReactionRanking> TopReactedPortfolios { get; set; } = new();
    }
}