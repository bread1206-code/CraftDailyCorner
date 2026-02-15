using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.ReportCommentRe;

namespace CraftDailyCorner.Services.Interface
{
    public interface IPostCommentReportService
    {
        ReportCommentResponse CreateReport(string commentId,string memberId,CommentReportReason reasonCode,string? description);
    }
}
