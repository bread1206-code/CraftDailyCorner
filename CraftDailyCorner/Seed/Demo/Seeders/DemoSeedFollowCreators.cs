using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedFollowCreators
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedFollowCreators(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Follows == null || !seedContext.Follows.Any())
                throw new Exception("DemoSeedContext.Follows 沒有資料");

            var existingMembers = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var existingCreators = _context.CreatorProfiles
                .Select(x => x.CreatorID)
                .ToHashSet();

            var existingFollowKeys = _context.FollowCreators
                .Select(x => new { x.MemberID, x.CreatorID })
                .ToHashSet();

            // 新增：避免同一批 seed 重複加入相同 (MemberID, CreatorID)
            var pendingFollowKeys = new HashSet<string>();

            var follows = new List<FollowCreator>();

            foreach (var row in seedContext.Follows)
            {
                if (!seedContext.BrandNameToCreatorMap.TryGetValue(row.BrandName, out var creatorId))
                    throw new Exception($"Follows.csv 找不到對應品牌：{row.BrandName}");

                if (!existingCreators.Contains(creatorId))
                    throw new Exception($"Follows.csv 找不到對應 CreatorID：{creatorId}");

                if (!existingMembers.Contains(row.MemberID))
                    throw new Exception($"Follows.csv 找不到對應 MemberID：{row.MemberID}");

                // 不追蹤自己
                if (seedContext.CreatorToMemberMap.TryGetValue(creatorId, out var creatorMemberId) &&
                    creatorMemberId == row.MemberID)
                {
                    continue;
                }

                var key = new
                {
                    row.MemberID,
                    CreatorID = creatorId
                };

                if (existingFollowKeys.Contains(key))
                    continue;

                // 新增：同一批資料若重複，也跳過
                var pendingKey = $"{row.MemberID}_{creatorId}";
                if (!pendingFollowKeys.Add(pendingKey))
                    continue;

                if (!seedContext.CreatorConfirmedAtMap.TryGetValue(creatorId, out var confirmedAt))
                    throw new Exception($"找不到 CreatorConfirmedAt：{creatorId}");

                var createdAt = confirmedAt.AddDays((follows.Count % 60) + 1);

                follows.Add(new FollowCreator
                {
                    MemberID = row.MemberID,
                    CreatorID = creatorId,
                    CreatedAt = createdAt
                });
            }

            if (follows.Any())
            {
                _context.FollowCreators.AddRange(follows);
                _context.SaveChanges();
            }
        }
    }
}