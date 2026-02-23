namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorOrderDetailItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductNameSnapshot { get; set; } = null!;
        public decimal PriceSnapshot { get; set; }
        public decimal CostSnapshot { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => PriceSnapshot * Quantity;
        public decimal Profit => (PriceSnapshot - CostSnapshot) * Quantity;
    }
}
