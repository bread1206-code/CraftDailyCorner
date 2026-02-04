using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class ProductTag
    {
        [Display(Name = "商品編號")]
        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        public string ProductID { get; set; }= null!;

        [Display(Name = "標籤編號")]
        public int TagID { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
