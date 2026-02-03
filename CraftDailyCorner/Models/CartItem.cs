using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class CartItem
    {
        [Display(Name = "購物車編號")]
        public int CartID { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; }= null!;

        [Display(Name = "數量")]
        public int Quantity { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
