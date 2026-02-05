using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
    //使用者「前往付款頁」時用
    public class VMPaymentCreate
    {
        // 訂單資訊
        public string OrderID { get; set; }
        public decimal OrderAmount { get; set; }

        // 顯示用
        public string OrderNo { get; set; }
        public DateTime OrderCreatedAt { get; set; }

        // 付款方式
        public byte SelectedMethodID { get; set; }
        public List<VMPaymentMethod> PaymentMethods { get; set; } = new();

        // Mock 用
        public bool IsMockPayment { get; set; } = true;
    }
}
