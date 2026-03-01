namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce
{
    public class VMCommerceDashboard
    {
        public VMCommerceOverview Overview { get; set; } = new();
        public VMCommerceRevenueTrend RevenueTrend { get; set; } = new();
        public VMCommerceOrderTrend OrderTrend { get; set; } = new();
        public VMCommerceProductRanking ProductRanking { get; set; } = new();
    }
}