using CraftDailyCorner.Services.CraftDailyCorner.Services.PostCommentReport;

namespace CraftDailyCorner.Services.Interface
{
    public interface IPostCommentReportService
    {
        ReportCommentResponse CreateReport(string commentId,string memberId,string reason);
    }
}
