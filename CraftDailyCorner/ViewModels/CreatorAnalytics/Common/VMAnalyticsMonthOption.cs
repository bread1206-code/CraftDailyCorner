namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Common
{
    public class VMAnalyticsMonthOption
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string Value => $"{Year:D4}-{Month:D2}";
        public string Text => $"{Year:D4}/{Month:D2}";
    }
}