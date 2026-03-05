using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace CraftDailyCorner.Models
{
    public class Report
    {
        [Key]
        [Display(Name = "檢舉ID")]
        public long ReportID { get; set; }
        
        [Required]
        [Display(Name = "檢舉類型")]
        public ReportTargetType ReportType { get; set; } //類型（日誌、留言、商品、作品集）

        // 目標ID（日誌、留言、商品、作品集）
        [Required]
        [StringLength(36)]
        public string TargetID { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public ReportReason ReasonCode { get; set; }

        //補充說明（只有選其他才填）
        [StringLength(200)]
        public string? Description { get; set; }


        [Display(Name = "檢舉者ID")]
        [Column(TypeName = "nchar(8)")]
        public string MemberID { get; set; } = null!;
        [Display(Name = "檢舉狀態ID")]
        public byte StatusID { get; set; }
        //建立時間
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "處理者")]
        [Column(TypeName = "nchar(8)")]
        public string? ReviewedBy { get; set; }
        [Display(Name = "處理時間")]
        public DateTime? ReviewedAt { get; set; }


        public virtual ReportStatus? ReportStatus { get; set; }

        // 檢舉人
        public virtual Member Reporter { get; set; } = null!;

        // 審核人
        public virtual Member? Reviewer { get; set; }

    }
}
