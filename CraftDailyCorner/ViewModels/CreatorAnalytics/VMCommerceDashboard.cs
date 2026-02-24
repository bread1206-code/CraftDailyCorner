namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommerceDashboard
    {
        public VMCommerceOverview Overview { get; set; } = new();
        public VMCommerceRevenueTrend RevenueTrend { get; set; } = new();
        public VMCommerceProductRanking ProductRanking { get; set; } = new();
    }
}
