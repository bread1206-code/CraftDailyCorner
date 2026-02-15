using CraftDailyCorner.Models;
using CraftDailyCorner.Services.CraftDailyCorner.Services.PostCommentReport;
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

        public ReportCommentResponse CreateReport(
    string commentId,
    string memberId,
    string reason)
        {
            var comment = _context.PostComments
                .Include(c => c.CreatorPost)
                .FirstOrDefault(c => c.CommentID == commentId);

            if (comment == null)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.NotFound
                };

            // 🔹 先找目前登入者的 CreatorProfile
            var creatorProfile = _context.CreatorProfiles
                .FirstOrDefault(c => c.MemberID == memberId);

            if (creatorProfile == null)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.Forbidden
                };

            // 🔹 比對 CreatorID
            if (comment.CreatorPost.CreatorID != creatorProfile.CreatorID)
                return new ReportCommentResponse
                {
                    Result = ReportCommentResult.Forbidden
                };

            var report = new PostCommentReport
            {
                CommentID = commentId,
                MemberID = memberId,
                Reason = reason,
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
