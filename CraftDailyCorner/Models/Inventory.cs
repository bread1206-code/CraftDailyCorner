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
        public int StockQty { get; set; }

        [Display(Name = "警戒值")]
        public int AlertQty { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual List<InventoryAlert>? InventoryAlerts { get; set; }
    }
}
