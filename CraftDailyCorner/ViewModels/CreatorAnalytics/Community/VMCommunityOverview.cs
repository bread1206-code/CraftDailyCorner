namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMCommunityOverview
    {
        public int TotalPosts { get; set; }
        public int PublishedPosts { get; set; }
        public int DraftPosts { get; set; }

        public int TotalPortfolios { get; set; }

        public int TotalComments { get; set; }

        public int TotalReactions { get; set; }

        // 本月 KPI
        public int PostsThisMonth { get; set; }
        public int PortfoliosThisMonth { get; set; }
        public int CommentsThisMonth { get; set; }
        public int ReactionsThisMonth { get; set; }
    }
}