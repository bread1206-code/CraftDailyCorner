using CraftDailyCorner.Models;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.ReportCommentRe;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class PostCommentReportService : IPostCommentReportService
    {
        private readonly CraftDailyCornerContext _context;

        public PostCommentReportService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public ReportCommentResponse CreateReport(string commentId,string memberId,CommentReportReason reasonCode,string? description)
        {
            var comment = _context.PostComments
        .Include(c => c.CreatorPost)
        .FirstOrDefault(c => c.CommentID == commentId);

            if (comment == null)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.NotFound
                };

            // 🔹 找目前登入者的 CreatorProfile
            var creatorProfile = _context.CreatorProfiles
                .FirstOrDefault(c => c.MemberID == memberId);

            if (creatorProfile == null)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.Forbidden
                };

            // 🔹 驗證是否為該文章作者
            if (comment.CreatorPost.CreatorID != creatorProfile.CreatorID)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.Forbidden
                };

            // 🔹 防止重複檢舉
            var exists = _context.PostCommentReports
                .Any(r =>
                    r.CommentID == commentId &&
                    r.MemberID == memberId);

            if (exists)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.AlreadyReported,
                    PostId = comment.PostID
                };

            var report = new PostCommentReport
            {
                CommentID = commentId,
                MemberID = memberId,
                ReasonCode = reasonCode,
                Description =
                    reasonCode == CommentReportReason.Other
                        ? description?.Trim()
                        : null,
                StatusID = 1
            };

            _context.PostCommentReports.Add(report);
            _context.SaveChanges();

            return new ReportCommentResponse
            {
                Result = ReportCommentResult.Success,
                PostId = comment.PostID
            };
        }
    }
}
