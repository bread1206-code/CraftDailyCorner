using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class ProductCategory
    {
        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        [Display(Name = "分類編號")]
        public int CategoryID { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
