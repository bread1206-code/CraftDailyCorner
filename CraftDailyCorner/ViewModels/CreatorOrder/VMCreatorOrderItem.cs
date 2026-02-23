namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorOrderItem
    {
        public string OrderID { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        public bool HasShipment { get; set; }
        public string? TrackingNo { get; set; }

        // UI 判斷用
        public bool CanProcess => StatusID == 2;     // Paid
        public bool CanShip => StatusID == 3;        // Processing
    }
}
