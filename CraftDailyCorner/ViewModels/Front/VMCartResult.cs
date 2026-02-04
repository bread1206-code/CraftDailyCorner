namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        // 庫存相關提示（可選）
        public int? StockQty { get; set; }

        // 操作後最新摘要（可選）
        public VMCartSummary? Summary { get; set; }
    }
}
