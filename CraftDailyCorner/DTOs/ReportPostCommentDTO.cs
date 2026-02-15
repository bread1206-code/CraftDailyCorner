using CraftDailyCorner.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.DTOs
{
    public class ReportPostCommentDTO
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string CommentID { get; set; } = null!;

        public CommentReportReason ReasonCode { get; set; }

        [Required(ErrorMessage = "請填寫檢舉原因")]
        [StringLength(200, ErrorMessage = "原因不可超過 200 字")]
        public string? Description { get; set; }
    }
}