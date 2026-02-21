using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.EntityFrameworkCore;


namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPostCommentService : ICreatorPostCommentService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorPostCommentService(
            CraftDailyCornerContext context)
        {
            _context = context;
        }

        //建立留言
        public async Task<VMPostCommentItem> CreateAsync(
            CreatePostCommentDTO dto,
            string memberId,
            string? creatorId = null)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == dto.PostID &&
                    p.StatusID == 1);

            if (post == null)
                throw new Exception("日誌不存在");

            var content = dto.Content.Trim();

            var comment = new PostComment
            {
                CommentID = Guid.NewGuid().ToString(),
                PostID = dto.PostID,
                MemberID = memberId,
                Content = content,
                Status = PostCommentStatus.Visible,
                CreatedAt = DateTime.Now
            };

            _context.PostComments.Add(comment);
            await _context.SaveChangesAsync();

            return await BuildCommentViewModelAsync(
                comment.CommentID,
                memberId,
                creatorId);
        }

        //取得留言列表
        public async Task<List<VMPostCommentItem>> GetPostCommentsAsync(string postId,string? currentMemberId,string? currentCreatorId)
        {
            return await _context.PostComments
                .Include(c => c.Member)
                .Include(c => c.CreatorPost)
                .Where(c =>
                    c.PostID == postId &&
                    c.Status == PostCommentStatus.Visible)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new VMPostCommentItem
                {
                    CommentID = c.CommentID,
                    PostID = c.PostID,
                    MemberID = c.MemberID,
                    MemberName = c.Member.DisplayName,
                    MemberAvatar = c.Member.ImageUrl,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status,
                    IsOwner = c.MemberID == currentMemberId
            }).ToListAsync();
        }


        

        //建構留言
        public async Task<VMPostCommentItem>
            BuildCommentViewModelAsync(
                string commentId,
                string? currentMemberId,
                string? currentCreatorId)
        {
            return await _context.PostComments
                .Include(c => c.Member)
                .Include(c => c.CreatorPost)
                .Where(c => c.CommentID == commentId)
                .Select(c => new VMPostCommentItem
                {
                    CommentID = c.CommentID,
                    PostID = c.PostID,
                    MemberID = c.MemberID,
                    MemberName = c.Member.DisplayName,
                    MemberAvatar = c.Member.ImageUrl,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status,
                    IsOwner = c.MemberID == currentMemberId
                })
                .FirstAsync();
        }
    }
}