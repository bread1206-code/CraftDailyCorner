using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Inventory
    {
        [Key]
        [Display(Name = "庫存編號")]
        public int InventoryID { get; set; }

        [Display(Name = "庫存數量")]
        public short StockQty { get; set; }

        [Display(Name = "警戒值")]
        public short AlertQty { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
        public virtual List<InventoryAlert>? InventoryAlerts { get; set; }
    }
}
