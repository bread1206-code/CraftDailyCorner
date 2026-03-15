using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedReactions
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedReactions(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Reactions == null || !seedContext.Reactions.Any())
                throw new Exception("DemoSeedContext.Reactions 沒有資料");

            var existingMembers = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var existingPosts = _context.CreatorPosts
                .Select(x => x.PostID)
                .ToHashSet();

            var existingReactionKeys = _context.Reactions
                .Where(x => x.TargetType == ReactionTargetType.CreatorPost)
                .Select(x => new { x.MemberID, x.TargetID })
                .ToHashSet();

            var reactions = new List<Reaction>();

            foreach (var row in seedContext.Reactions)
            {
                if (!seedContext.CsvPostIdToDbPostIdMap.TryGetValue(row.CsvPostID, out var postId))
                    throw new Exception($"Reactions.csv 找不到對應 CsvPostID：{row.CsvPostID}");

                if (!existingPosts.Contains(postId))
                    throw new Exception($"Reactions.csv 找不到對應 PostID：{postId}");

                if (!existingMembers.Contains(row.MemberID))
                    throw new Exception($"Reactions.csv 找不到對應 MemberID：{row.MemberID}");

                var key = new
                {
                    row.MemberID,
                    TargetID = postId
                };

                // 同一會員對同一篇貼文只保留一筆 reaction
                if (existingReactionKeys.Contains(key))
                    continue;

                if (!seedContext.PostCreatedAtMap.TryGetValue(postId, out var postCreatedAt))
                    throw new Exception($"找不到 PostCreatedAt：{postId}");

                var createdAt = postCreatedAt.AddHours((reactions.Count % 72) + 1);

                reactions.Add(new Reaction
                {
                    TargetType = ReactionTargetType.CreatorPost,
                    TargetID = postId,
                    ReactionType = ParseReactionType(row.ReactionType),
                    CreatedAt = createdAt,
                    MemberID = row.MemberID
                });
            }

            if (reactions.Any())
            {
                _context.Reactions.AddRange(reactions);
                _context.SaveChanges();
            }
        }

        private static ReactionType ParseReactionType(string value)
        {
            var text = value?.Trim().ToLowerInvariant();

            return text switch
            {
                "like" => ReactionType.Like,
                "love" => ReactionType.Love,
                "haha" => ReactionType.Haha,
                "wow" => ReactionType.Wow,
                "sad" => ReactionType.Sad,
                "angry" => ReactionType.Angry,
                _ => ReactionType.Like
            };
        }
    }
}