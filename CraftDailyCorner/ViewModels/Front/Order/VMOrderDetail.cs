namespace CraftDailyCorner.ViewModels.Front.Order
{
    //訂單完成頁 / 訂單查詢頁
    public class VMOrderDetail
    {
        public string OrderID { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ReceiverAddress { get; set; } = null!;

        public int TotalAmount { get; set; }

        public List<VMOrderItem> Items { get; set; } = new();
    }
}
