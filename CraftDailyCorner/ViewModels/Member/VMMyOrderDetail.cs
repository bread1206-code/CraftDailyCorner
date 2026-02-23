using CraftDailyCorner.ViewModels.Payment;

namespace CraftDailyCorner.ViewModels.Member
{
    //顯示訂單基本資訊
    public class VMMyOrderDetail
    {
        // ===== 訂單基本資訊 =====
        public string OrderID { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int StatusID { get; set; }
        public string StatusText { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        // ===== 收件資訊 =====
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        // ===== 商品明細 =====
        public List<VMMyOrderItem> Items { get; set; } = new();

        // ===== 付款紀錄 =====
        public VMOrderPaymentList OrderPayments { get; set; } = new();
    }
}
