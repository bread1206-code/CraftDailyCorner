namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMProductSalesRanking
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;

        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
