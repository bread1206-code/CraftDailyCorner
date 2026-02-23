namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorOrderList
    {
        public string StatusFilter { get; set; } = null!;

        public List<VMCreatorOrderItem> Orders { get; set; } = new();

        // 分頁用（未來可擴充）
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
