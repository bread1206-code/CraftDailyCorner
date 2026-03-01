using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Reaction;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class ReactionService : IReactionService
    {
        private readonly CraftDailyCornerContext _context;

        public ReactionService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<ReactionResultDTO> ToggleAsync(
            string memberId,
            ReactionTargetType targetType,
            string targetId,
            ReactionType reactionType)
        {
            var reaction = await _context.Reactions
                .FirstOrDefaultAsync(r =>
                    r.MemberID == memberId &&
                    r.TargetType == targetType &&
                    r.TargetID == targetId);

            ReactionType? userReaction;

            if (reaction == null)
            {
                _context.Reactions.Add(new Reaction
                {
                    MemberID = memberId,
                    TargetType = targetType,
                    TargetID = targetId,
                    ReactionType = reactionType,
                    CreatedAt = DateTime.Now
                });

                userReaction = reactionType;
            }
            else if (reaction.ReactionType == reactionType)
            {
                _context.Reactions.Remove(reaction);
                userReaction = null;
            }
            else
            {
                reaction.ReactionType = reactionType;
                userReaction = reactionType;
            }

            await _context.SaveChangesAsync();

            var grouped = await _context.Reactions
                .Where(r => r.TargetType == targetType &&
                            r.TargetID == targetId)
                .GroupBy(r => r.ReactionType)
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return new ReactionResultDTO
            {
                Reactions = grouped.ToDictionary(x => x.Type, x => x.Count),
                UserReactionType = userReaction
            };
        }

        public async Task<VMReactionButton> GetButtonStateAsync(
    string? memberId,
    ReactionTargetType targetType,
    string targetId)
        {
            var reactions = await _context.Reactions
                .Where(r => r.TargetType == targetType &&
                            r.TargetID == targetId)
                .ToListAsync();

            var total = reactions.Count;

            ReactionType? userReaction = null;

            if (!string.IsNullOrEmpty(memberId))
            {
                userReaction = reactions
                    .FirstOrDefault(r => r.MemberID == memberId)
                    ?.ReactionType;
            }

            return new VMReactionButton
            {
                TargetType = targetType,
                TargetID = targetId,
                UserReactionType = userReaction,
                TotalCount = total
            };
        }
    }
}
