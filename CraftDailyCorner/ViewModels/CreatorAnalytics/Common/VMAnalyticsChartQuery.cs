namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Common
{
    public class VMAnalyticsChartQuery
    {
        /// <summary>
        /// year / rolling12 / month
        /// </summary>
        public string Mode { get; set; } = CreatorAnalyticsChartModes.Year;

        /// <summary>
        /// 年度報表用，例如 2026
        /// 單月報表也會用到
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 單月報表用，例如 3 代表 3 月
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 近12個月模式的結束年份，例如 2026
        /// </summary>
        public int? EndYear { get; set; }

        /// <summary>
        /// 近12個月模式的結束月份，例如 3 代表 2026/03
        /// </summary>
        public int? EndMonth { get; set; }
    }
}