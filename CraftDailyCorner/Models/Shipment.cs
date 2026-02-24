using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Shipment
    {
        [Key]
        [Display(Name = "運輸編號")]
        public int ShipmentID { get; set; }

        [StringLength(50)]
        [Display(Name = "物流編號")]
        public string TrackingNo { get; set; } = null!;

        [Display(Name = "狀態")]
        public byte StatusID { get; set; } = 0;

        [Display(Name ="出貨時間")]
        public DateTime? ShippedAt { get; set; }

        [StringLength(12, MinimumLength = 12)]
        [Column(TypeName = "nchar(12)")]
        [Display(Name = "訂單編號")]
        public string OrderID { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual ShipmentStatus ShipmentStatus { get; set; } = null!;
    }
}
