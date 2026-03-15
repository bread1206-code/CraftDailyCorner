using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedPostComments
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedPostComments(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.PostComments == null || !seedContext.PostComments.Any())
                throw new Exception("DemoSeedContext.PostComments 沒有資料");

            var existingMembers = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var existingPosts = _context.CreatorPosts
                .Select(x => x.PostID)
                .ToHashSet();

            var existingCommentKeys = _context.PostComments
                .Select(x => new { x.PostID, x.MemberID, x.Content })
                .ToHashSet();

            var comments = new List<PostComment>();

            foreach (var row in seedContext.PostComments)
            {
                if (!seedContext.CsvPostIdToDbPostIdMap.TryGetValue(row.CsvPostID, out var postId))
                    throw new Exception($"PostComments.csv 找不到對應 CsvPostID：{row.CsvPostID}");

                if (!existingPosts.Contains(postId))
                    throw new Exception($"PostComments.csv 找不到對應 PostID：{postId}");

                if (!existingMembers.Contains(row.MemberID))
                    throw new Exception($"PostComments.csv 找不到對應 MemberID：{row.MemberID}");

                var key = new
                {
                    PostID = postId,
                    row.MemberID,
                    row.Content
                };

                if (existingCommentKeys.Contains(key))
                    continue;

                if (!seedContext.PostCreatedAtMap.TryGetValue(postId, out var postCreatedAt))
                    throw new Exception($"找不到 PostCreatedAt：{postId}");

                var createdAt = postCreatedAt.AddHours((comments.Count % 48) + 1);

                comments.Add(new PostComment
                {
                    CommentID = Guid.NewGuid().ToString(),
                    Content = row.Content,
                    Status = PostCommentStatus.Visible,
                    CreatedAt = createdAt,
                    PostID = postId,
                    MemberID = row.MemberID
                });
            }

            if (comments.Any())
            {
                _context.PostComments.AddRange(comments);
                _context.SaveChanges();
            }
        }
    }
}