namespace CraftDailyCorner.ViewModels.Front.Cart
{
    //購物車摘要資訊，例如：總件數、總金額，用在 Navbar Badge 或 Modal 上方顯示
    public class VMCartSummary
    {
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
