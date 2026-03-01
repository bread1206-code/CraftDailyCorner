using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class ReactionResultDTO
    {
        public Dictionary<ReactionType, int> Reactions { get; set; } = new();

        public int TotalCount { get; set; }
        public ReactionType? TopReactionType { get; set; }

        public ReactionType? UserReactionType { get; set; }
    }
}
