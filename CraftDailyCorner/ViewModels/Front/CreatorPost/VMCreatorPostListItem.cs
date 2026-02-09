namespace CraftDailyCorner.ViewModels.Front.CreatorPost
{
    public class VMCreatorPostListItem
    {
        public string PostID { get; set; } = null!;

        public string Title { get; set; } = null!;

        public CreatorPostVisibility Visibility { get; set; }

        public DateTime CreatedAt { get; set; }

        // UI 用（平台是否隱藏）
        public bool IsHidden { get; set; }
    }
}