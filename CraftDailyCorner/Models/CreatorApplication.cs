using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class CreatorApplication
    {
        [Key]
        [Display(Name = "申請表編號")]
        public int ApplicationID { get; set; }

        [Required]
        [StringLength(20,MinimumLength =1)]
        [Display(Name = "品牌名稱")]
        public string DisplayName { get; set; } = null!;

        [Required]
        [Display(Name = "品牌簡介")]
        [Column(TypeName = "nvarchar(max)")]
        public string Intro { get; set; } = null!;

        [Required]
        [StringLength(36,MinimumLength =36)]
        [Column(TypeName ="nchar(36)")]
        [Display(Name = "作品圖片URL")]
        public string PortfolioSampleUrl { get; set; } = null!;

        [Required]
        [Display(Name = "創作起始日")]
        public DateTime StartDate { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        [Display(Name = "申請時間")]
        public DateTime AppliedAt { get; set; }

        [Display(Name = "審核時間")]
        public DateTime? ReviewedAt { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "審核備註")]
        public string? ReviewNote { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!; 

        [StringLength(8,MinimumLength =8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "審核者")]
        public string? ReviewedBy { get; set; }

        // 導覽屬性
        [Display(Name = "申請會員")]
        public virtual Member Member { get; set; } = null!;

        [Display(Name = "審核會員")]
        public virtual Member Reviewer { get; set; } = null!;
        public virtual CreatorApplicationStatus CreatorApplicationStatus { get; set; } = null!;
    }
}
