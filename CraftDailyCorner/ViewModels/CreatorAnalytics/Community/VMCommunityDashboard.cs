namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMCommunityDashboard
    {
        public VMCommunityOverview Overview { get; set; } = new();
        public VMCommunityContentAnalysis ContentAnalysis { get; set; } = new();
        public VMCommunityPortfolioAnalysis PortfolioAnalysis { get; set; } = new();
        public VMCommunityInteractionAnalysis InteractionAnalysis { get; set; } = new();
        public VMCommunityReactionAnalysis ReactionAnalysis { get; set; } = new();
    }
}