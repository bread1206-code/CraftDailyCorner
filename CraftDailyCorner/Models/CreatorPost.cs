using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class CreatorPost
    {
        [Key]
        [StringLength(36,MinimumLength =36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "日誌編號")]
        public string PostID { get; set; } = null!;
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "內容")]
        public string Content { get; set; } = null!;
        [Required]
        [Column(TypeName = "nchar(36)")]
        [Display(Name ="日誌圖片")]
        public string ImageUrl { get; set; } = null!;
        [Display(Name = "誰可以看")]
        public CreatorPostVisibility Visibility { get; set; }
        [Display(Name = "狀態")]
        public byte StatusID { get; set; } = 0;
        [Display(Name = "建立日期")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "更新日期")]
        public DateTime UpdatedAt { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; } = null!;

        // 導覽屬性
        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
        public virtual List<PostComment>? PostComments { get; set; } = new();
        public virtual CreatorPostStatus CreatorPostStatus { get; set; } = null!;
    }
}
