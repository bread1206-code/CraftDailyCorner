namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMPostCommentRanking
    {
        public string PostID { get; set; } = null!;
        public string Title { get; set; } = null!;

        public int CommentCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
