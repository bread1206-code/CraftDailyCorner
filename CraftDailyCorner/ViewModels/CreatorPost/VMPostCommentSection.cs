namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMPostCommentSection
    {
        public string PostID { get; set; } = null!;
        public bool CanComment { get; set; }
        public List<VMPostCommentItem> Comments { get; set; } = new();
    }
}