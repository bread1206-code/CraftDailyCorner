using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Models
{
    public class PostComment
    {
        [Key]
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name ="留言編號")]
        public string CommentID { get; set; } = null!;
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "留言內容")]
        public string Content { get; set; } = null!;
        [Display(Name = "狀態")]
        public PostCommentStatus Status { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "日誌編號")]
        public string PostID { get; set; } = null!;

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        public virtual CreatorPost CreatorPost { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
        //public virtual List<Report>? PostCommentReports { get; set; }
        }
}
