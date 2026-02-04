using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class OrderDetail
    {
        [StringLength(12, MinimumLength = 12)]
        [Column(TypeName = "nchar(12)")]
        [Display(Name = "訂單編號")]
        public string OrderID { get; set; }= null!;

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; }= null!;

        [StringLength(40, MinimumLength = 1)]
        [Display(Name = "產品名稱快照")]
        public string ProductNameSnapshot { get; set; }= null!;

        [Display(Name = "金額快照")]
        [Column(TypeName = "money")]
        public decimal PriceSnapshot { get; set; }

        [Display(Name = "數量")]
        public int Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
