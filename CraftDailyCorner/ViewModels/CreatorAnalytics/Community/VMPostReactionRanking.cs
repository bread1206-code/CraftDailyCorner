namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMPostReactionRanking
    {
        public string PostID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int ReactionCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}