namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class OrderDetailSeedRow
    {
        public string OrderID { get; set; } = null!;
        public string ProductID { get; set; } = null!;
        public string ProductNameSnapshot { get; set; } = null!;
        public decimal PriceSnapshot { get; set; }
        public decimal CostSnapshot { get; set; }
        public int Quantity { get; set; }
    }
}