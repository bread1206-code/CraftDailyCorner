namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorOrderDetail
    {
        public string OrderID { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public decimal TotalAmount { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;
        public string? SuggestedTrackingNo { get; set; }
        public string? TrackingNo { get; set; }

        public List<VMCreatorOrderDetailItem> Items { get; set; } = new();
    }
}
