namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMPortfolioReactionRanking
    {
        public string PortfolioID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int ReactionCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}