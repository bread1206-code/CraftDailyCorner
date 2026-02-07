using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class CreatorProfile
    {
        [Key]
        [StringLength(6,MinimumLength =6)]
        [Column(TypeName ="nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; } = null!;

        [StringLength(36,MinimumLength =36)]
        [Column(TypeName ="nchar(36)")]
        [Display(Name = "品牌圖片")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [StringLength(40, MinimumLength = 1)]
        [Display(Name = "創作者暱稱")]
        public string DisplayName { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "創作者簡介")]
        public string Intro { get; set; } = null!;

        [Required]
        [Display(Name = "創作起始日")]
        public DateTime StartDate { get; set; }

        [StringLength(3)]
        [Column(TypeName ="nchar(3)")]
        [Display(Name = "銀行代碼")]
        public string? BankCode { get; set; }

        [Display(Name = "銀行帳號")]
        [StringLength(14, MinimumLength = 8, ErrorMessage = "銀行帳號長度必須介於8到14個字元之間。")]
        [Column(TypeName = "nvarchar(14)")]
        public string? BankAccount { get; set; } 

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(8,MinimumLength =8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        // 導覽屬性
        [Display(Name = "會員")]
        public virtual Member Member { get; set; } = null!;
        public virtual List<Product>? Products { get; set; }
        public virtual List<AutoReplyTemplate>? AutoReplyTemplates { get; set; }
        public virtual List<CreatorPost>? CreatorPosts { get; set; }
        public virtual List<Portfolio>? Portfolios { get; set; }
        public virtual List<FollowCreator>? FollowCreators { get; set; }
        public virtual List<MessageThread>? MessageThreads { get; set; }
        public virtual CreatorProfileStatus? CreatorProfileStatus { get; set; }
    }
}
