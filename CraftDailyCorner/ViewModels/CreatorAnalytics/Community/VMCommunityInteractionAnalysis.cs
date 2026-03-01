namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMCommunityInteractionAnalysis
    {
        public int CommentsThisMonth { get; set; }
        public int CommentsLastMonth { get; set; }

        public decimal CommentGrowthRate { get; set; }

        public List<VMPostCommentRanking> TopCommentPosts { get; set; } = new();

        public List<VMCommentMonthlyTrend> CommentTrend { get; set; } = new();
    }
}
