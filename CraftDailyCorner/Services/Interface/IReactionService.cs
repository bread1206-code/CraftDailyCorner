using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.ViewModels.Reaction;

namespace CraftDailyCorner.Services.Interface
{
    public interface IReactionService
    {
        Task<ReactionResultDTO> ToggleAsync(
            string memberId,
            ReactionTargetType targetType,
            string targetId,
            ReactionType reactionType);

        Task<VMReactionButton> GetButtonStateAsync(
            string? memberId,
            ReactionTargetType targetType,
            string targetId);
    }
}
