namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommunityDashboard
    {
        public VMCommunityOverview Overview { get; set; } = new();
        public VMCommunityContentAnalysis ContentAnalysis { get; set; } = new();
        public VMCommunityInteractionAnalysis InteractionAnalysis { get; set; } = new();
        public VMCommunityBusinessAnalysis BusinessAnalysis { get; set; } = new();
    }
}
