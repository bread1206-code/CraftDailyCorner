using CraftDailyCorner.ImageManagementCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class ProductImage : IEntityImage
    {
        [Key]
        [Display(Name = "圖片編號")]
        public long ImageID { get; set; } 

        [Display(Name = "圖片URL")]
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "排序")]
        public byte SortOrder { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
        public virtual ProductImageStatus ProductImageStatus { get; set; } = null!;

        public string EntityID => ProductID;
    }
}
