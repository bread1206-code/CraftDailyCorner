namespace CraftDailyCorner.ViewModels.Front.Member
{
    //顯示「我下過哪些訂單」
    public class VMMyOrder
    {
        public string OrderID { get; set; } = null!;   // 訂單編號（對外顯示 & 路由用）
        public DateTime CreatedAt { get; set; }        // 下單時間
        public int TotalAmount { get; set; }           // 總金額（整數顯示）
        public string StatusText { get; set; } = null!;// 訂單狀態文字
    }
}
