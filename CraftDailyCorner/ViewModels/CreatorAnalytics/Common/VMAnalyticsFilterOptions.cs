namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Common
{
    public class VMAnalyticsFilterOptions
    {
        public List<int> AvailableYears { get; set; } = new();
        public List<VMAnalyticsMonthOption> AvailableMonths { get; set; } = new();
    }
}