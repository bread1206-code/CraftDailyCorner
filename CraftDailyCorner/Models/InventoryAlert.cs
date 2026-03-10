using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Models
{
    public class InventoryAlert
    {
        [Key]
        [Display(Name = "提醒編號")]
        public int AlertID { get; set; }

        [Display(Name = "觸發時間")]
        public DateTime TriggeredAt { get; set; }

        [Display(Name = "狀態")]
        public InventoryAlertStatus Status { get; set; }

        [Display(Name = "庫存編號")]
        public int InventoryID { get; set; }

        public virtual Inventory Inventory { get; set; } = null!;
    }
}
