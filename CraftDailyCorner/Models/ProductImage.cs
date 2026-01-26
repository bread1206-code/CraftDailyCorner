using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class ProductImage
    {
        [Key]
        [Display(Name = "圖片編號")]
        public long ImageID { get; set; } 

        [Display(Name = "圖片URL")]
        [StringLength(40, MinimumLength = 40)]
        [Column(TypeName = "nchar(40)")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "排序")]
        public byte SortOrder { get; set; } 

        [Display(Name = "狀態")]
        public ProductImageStatus Status { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
