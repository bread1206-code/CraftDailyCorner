using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Reaction
{
    public class VMReactionButton
    {
        public ReactionTargetType TargetType { get; set; }
        public required string TargetID { get; set; }

        // 使用者自己的狀態（留言右側按鈕高亮用）
        public ReactionType? UserReactionType { get; set; }

        // 標題旁 summary 用
        public ReactionType? TopReactionType { get; set; }
        public int TotalCount { get; set; }
    }
}
