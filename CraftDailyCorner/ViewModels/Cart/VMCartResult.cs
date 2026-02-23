namespace CraftDailyCorner.ViewModels.Cart
{
    //購物車操作回傳結果用（Add / Remove / Update），通常包含：是否成功、訊息、目前購物車數量、總金額等（給 AJAX / API 用）
    public class VMCartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // 庫存相關提示（可選）
        public int? StockQty { get; set; }

        // 操作後最新摘要（可選）
        public VMCartSummary? Summary { get; set; }
    }
}
