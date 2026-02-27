namespace CraftDailyCorner.Areas.Admin.ViewModels
{
    public class VMDashboard
    {
        // ===== KPI 區 =====

        public int TodayOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public int TodayMembers { get; set; }

        // 成長率（與昨日比較）
        public decimal OrderGrowthRate { get; set; }
        public decimal RevenueGrowthRate { get; set; }
        public decimal MemberGrowthRate { get; set; }

        // 是否成長（控制箭頭方向）
        public bool OrderGrowthUp { get; set; }
        public bool RevenueGrowthUp { get; set; }
        public bool MemberGrowthUp { get; set; }

        // ===== 歷史月份下拉選單 =====

        // 例如：2026-01, 2025-12
        public List<string> AvailableMonths { get; set; } = new();
    }
}
