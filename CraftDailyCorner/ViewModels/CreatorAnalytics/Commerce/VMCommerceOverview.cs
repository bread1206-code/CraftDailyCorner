namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce
{
    public class VMCommerceOverview
    {
        public VMCommerceKpiDelta<decimal> Revenue { get; set; } = new();
        public VMCommerceKpiDelta<int> Orders { get; set; } = new();
        public VMCommerceKpiDelta<int> Quantity { get; set; } = new();

        public decimal AverageOrderValue { get; set; } // 你可定義成 本月 AOV 或 總體 AOV
    }
}