using CraftDailyCorner.ViewModels.Front;

namespace CraftDailyCorner.ViewModels
{
    //顯示訂單基本資訊
    public class VMMyOrderDetail
    {
        public string OrderID { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int TotalAmount { get; set; }
        public string StatusText { get; set; } = null!;

        // 收件資訊
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        // 訂單明細
        public List<VMMyOrderItem> Items { get; set; } = new();
    }
}
