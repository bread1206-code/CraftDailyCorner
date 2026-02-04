using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace CraftDailyCorner.Models
{
    public class PostCommentReport
    {
        [Key]
        public long ReportID { get; set; }
        public string Reason { get; set; } = null!;
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string CommentID { get; set; } = null!;
        public string MemberID { get; set; } = null!;
        public byte StatusID { get; set; }

        public virtual PostCommentReportStatus? PostCommentReportStatus { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual PostComment PostComment { get; set; } = null!;

    }
}
