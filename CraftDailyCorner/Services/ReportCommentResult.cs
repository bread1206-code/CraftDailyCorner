
namespace CraftDailyCorner.Services.ReportCommentRe
{
    public enum ReportCommentResult
    {
        Success,
        NotFound,
        Forbidden,
        AlreadyReported
    }

    public class ReportCommentResponse
    {
        public ReportCommentResult Result { get; set; }
        public string? PostId { get; set; }
    }
}

