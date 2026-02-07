using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class PortfolioItem
    {
        [Key]
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "作品編號")]
        public string ItemID { get; set; } = null!;

        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "圖片URL")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "簡介")]
        public string? Description { get; set; }

        [Display(Name = "排序")]
        public byte SortOrder { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "作品集編號")]
        public string PortfolioID { get; set; } = null!;

        public virtual Portfolio Portfolio { get; set; } = null!;
    }
}
