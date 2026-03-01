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
            var dict = grouped.ToDictionary(x => x.Type, x => x.Count);
            var totalCount = dict.Values.Sum();
            ReactionType? topType = null;

            if (dict.Count > 0)
            {
                topType = dict
                    .OrderByDescending(kv => kv.Value)
                    .ThenBy(kv => (byte)kv.Key)   // 避免同分時 icon 跳動
                    .First().Key;
            }

            return new ReactionResultDTO
            {
                Reactions = dict,
                TotalCount = totalCount,
                TopReactionType = topType,
                UserReactionType = userReaction
            };
        }


            public async Task<VMReactionButton> GetButtonStateAsync(
                string? memberId,
                ReactionTargetType targetType,
                string targetId)
        {
            // 1) 使用者自己的 reaction
            ReactionType? userReaction = null;
            if (!string.IsNullOrEmpty(memberId))
            {
                userReaction = await _context.Reactions
                    .Where(r => r.TargetType == targetType &&
                                r.TargetID == targetId &&
                                r.MemberID == memberId)
                    .Select(r => (ReactionType?)r.ReactionType)
                    .FirstOrDefaultAsync();
            }

            // 2) 統計 top + total
            var grouped = await _context.Reactions
                .Where(r => r.TargetType == targetType &&
                            r.TargetID == targetId)
                .GroupBy(r => r.ReactionType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var totalCount = grouped.Sum(x => x.Count);

            ReactionType? topType = null;
            if (grouped.Count > 0)
            {
                topType = grouped
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => (byte)x.Type)
                    .First().Type;
            }

            return new VMReactionButton
            {
                TargetType = targetType,
                TargetID = targetId,
                UserReactionType = userReaction,
                TotalCount = totalCount,
                TopReactionType = topType
            };
        }
    }
}
