using CraftDailyCorner.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace CraftDailyCorner.Models
{
    public class PostCommentReport
    {
        [Key]
        [Display(Name = "檢舉ID")]
        public long ReportID { get; set; }



        [Required]
        [StringLength(20)]
        public CommentReportReason ReasonCode { get; set; }

        //補充說明（只有選其他才填）
        [StringLength(300)]
        public string? Description { get; set; }


        [Display(Name = "處理者")]
        public string? ReviewedBy { get; set; }
        [Display(Name = "檢舉時間")]
        public DateTime? ReviewedAt { get; set; }


        [Display(Name = "留言ID")]
        public string CommentID { get; set; } = null!;
        [Display(Name = "檢舉者ID")]
        public string MemberID { get; set; } = null!;
        [Display(Name = "檢舉狀態ID")]
        public byte StatusID { get; set; }

        public virtual PostCommentReportStatus? PostCommentReportStatus { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual PostComment PostComment { get; set; } = null!;

    }
}
