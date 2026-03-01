namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Community
{
    public class VMPostCommentRanking
    {
        public string PostID { get; set; } = null!;
        public string Title { get; set; } = null!;

        public int CommentCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
