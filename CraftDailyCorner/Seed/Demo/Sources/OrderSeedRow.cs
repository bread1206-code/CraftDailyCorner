namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class OrderSeedRow
    {
        public string OrderID { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public byte StatusID { get; set; }
        public decimal TotalAmount { get; set; }
        public string MemberID { get; set; } = null!;
    }
}