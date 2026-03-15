namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class ShipmentSeedRow
    {
        public int ShipmentID { get; set; }
        public string TrackingNo { get; set; } = null!;
        public byte StatusID { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string OrderID { get; set; } = null!;
    }
}