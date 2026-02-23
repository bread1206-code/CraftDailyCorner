using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorShipmentUpdate
    {
        public string OrderID { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string TrackingNo { get; set; } = null!;
    }
}
