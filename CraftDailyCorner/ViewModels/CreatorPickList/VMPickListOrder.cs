namespace CraftDailyCorner.ViewModels.CreatorPickList
{
    // 撿貨單中的單筆訂單資訊
    public class VMPickListOrder
    {
        public string OrderID { get; set; } = null!;

        public string ReceiverName { get; set; } = null!;

        public string ReceiverPhone { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<VMPickListOrderItem> Items { get; set; } = new();

    }
}
