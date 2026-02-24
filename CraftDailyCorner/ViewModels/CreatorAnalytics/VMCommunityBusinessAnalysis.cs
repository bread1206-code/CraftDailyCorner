namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommunityBusinessAnalysis
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal AverageOrderValue { get; set; }

        public List<VMProductSalesRanking> TopSellingProducts { get; set; } = new();
    }
}
