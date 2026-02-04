namespace CraftDailyCorner.Services
{
    public class CartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CartQty { get; set; }
        public int StockQty { get; set; }
    }
}
