using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Reaction
{
    public class VMReactionButton
    {
        public ReactionTargetType TargetType { get; set; }
        public string TargetID { get; set; }

        public bool IsReacted { get; set; }
        public int ReactionCount { get; set; }
        public int TotalCount { get; set; }
        public ReactionType? UserReactionType { get; set; }
    }
}
