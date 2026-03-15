namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class PaymentSeedRow
    {
        public int PaymentID { get; set; }
        public byte MethodID { get; set; }
        public decimal Amount { get; set; }
        public byte StatusID { get; set; }
        public string GatewayTradeNo { get; set; } = null!;
        public byte AttemptNo { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderID { get; set; } = null!;
    }
}