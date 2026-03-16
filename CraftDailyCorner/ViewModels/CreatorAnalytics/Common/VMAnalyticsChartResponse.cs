namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Common
{
    public class VMAnalyticsChartResponse
    {
        /// <summary>
        /// 圖表標題，例如：月營收趨勢
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// year / rolling12 / month
        /// </summary>
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// 顯示目前查詢範圍，例如：
        /// 2026 年
        /// 2025/04 ～ 2026/03
        /// 2026/03
        /// </summary>
        public string RangeText { get; set; } = string.Empty;

        /// <summary>
        /// currency / count
        /// 前端可依此決定 tooltip 顯示格式
        /// </summary>
        public string ValueType { get; set; } = "count";

        /// <summary>
        /// 若該圖表有成長率可帶入，沒有可為 null
        /// </summary>
        public decimal? GrowthRate { get; set; }

        /// <summary>
        /// X 軸標籤
        /// </summary>
        public List<string> Labels { get; set; } = new();

        /// <summary>
        /// Y 軸數值
        /// 統一用 decimal，count 類型也可直接用整數轉 decimal
        /// </summary>
        public List<decimal> Values { get; set; } = new();
    }
}