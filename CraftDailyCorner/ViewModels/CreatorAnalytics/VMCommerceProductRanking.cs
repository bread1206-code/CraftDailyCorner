namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommerceProductRanking
    {
        public List<VMProductSalesRanking> TopByRevenue { get; set; } = new();
        public List<VMProductSalesRanking> TopByQuantity { get; set; } = new();
    }
}
