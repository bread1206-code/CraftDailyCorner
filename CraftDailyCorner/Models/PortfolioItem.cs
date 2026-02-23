using CraftDailyCorner.ImageManagementCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class PortfolioItem : IEntityImage
    {
        [Key]
        [Display(Name = "作品編號")]
        public long ItemID { get; set; } 

        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        [Display(Name = "圖片URL")]
        public string ImageUrl { get; set; } = null!;

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
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        public virtual Portfolio Portfolio { get; set; } = null!;

        public long ImageID => ItemID;
        public string EntityID => PortfolioID;
    }
}
