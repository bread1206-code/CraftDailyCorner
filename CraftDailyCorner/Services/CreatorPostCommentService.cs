using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Services.ReportCommentRe;

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

                    CanReport =
                        currentCreatorId != null &&
                        c.CreatorPost.CreatorID == currentCreatorId &&
                        c.MemberID != currentMemberId
            }).ToListAsync();
        }


        //檢舉留言
        public async Task<ReportCommentResponse> ReportAsync(ReportPostCommentDTO dto,string reporterId)
        {
            var comment = await _context.PostComments
                .Include(c => c.CreatorPost)
                .FirstOrDefaultAsync(c => c.CommentID == dto.CommentID);

            if (comment == null)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.NotFound
                };

            var creatorProfile = await _context.CreatorProfiles
                .FirstOrDefaultAsync(c => c.MemberID == reporterId);

            if (creatorProfile == null ||
                comment.CreatorPost.CreatorID != creatorProfile.CreatorID)
            {
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.Forbidden
                };
            }

            var exists = await _context.PostCommentReports
                .AnyAsync(r =>
                    r.CommentID == dto.CommentID &&
                    r.MemberID == reporterId);

            if (exists)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.AlreadyReported,
                    PostId = comment.PostID
                };

            var report = new PostCommentReport
            {
                CommentID = dto.CommentID,
                MemberID = reporterId,
                ReasonCode = dto.ReasonCode,
                Description =
                    dto.ReasonCode == CommentReportReason.Other
                        ? dto.Description?.Trim()
                        : null,
                StatusID = 1
            };

            _context.PostCommentReports.Add(report);
            await _context.SaveChangesAsync();

            return new ReportCommentResponse
            {
                Result = ReportCommentResult.Success,
                PostId = comment.PostID
            };
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

                    CanReport =
                        currentCreatorId != null &&
                        c.CreatorPost.CreatorID == currentCreatorId &&
                        c.MemberID != currentMemberId
                })
                .FirstAsync();
        }
    }
}