using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPostComment
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPostComment(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.PostComment.Any()) // 避免重複 Seed
            {
                var postComments = new List<PostComment>
                {
                    new PostComment
                    {
                        CommentID = Guid.NewGuid().ToString(),
                        Content = "真的很漂亮！",
                        Status = (PostCommentStatus)1,
                        CreatedAt = DateTime.Now,
                        PostID = imageGuids[0],
                        MemberID = "M0000002"
                    }
                };
                _context.PostComment.AddRange(postComments);
                _context.SaveChanges();
            }
        }
    }
}
