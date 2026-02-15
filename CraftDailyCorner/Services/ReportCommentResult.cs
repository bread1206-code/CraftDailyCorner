namespace CraftDailyCorner.Services
{
    namespace CraftDailyCorner.Services.PostCommentReport
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
}
