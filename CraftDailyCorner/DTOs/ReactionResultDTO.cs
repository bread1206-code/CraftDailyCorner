using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class ReactionResultDTO
    {
        public Dictionary<ReactionType, int> Reactions { get; set; }

        public ReactionType? UserReactionType { get; set; }
    }
}
