namespace CraftDailyCorner.ViewModels.Front.Creator
{
    public class VMCreatorDashboard
    {
        // 身分
        public string CreatorID { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        // 品牌圖片
        public string? ImageUrl { get; set; }

        // 品牌資訊
        public string Intro { get; set; } = null!;
        public DateTime StartDate { get; set; }

        // 系統資訊
        public DateTime CreatedAt { get; set; }

        // 統計（預留）
        public int ProductCount { get; set; }
        public int PortfolioCount { get; set; }
        public int CreatorPostCount { get; set; }
    }
}