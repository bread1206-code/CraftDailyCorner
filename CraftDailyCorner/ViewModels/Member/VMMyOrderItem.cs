namespace CraftDailyCorner.ViewModels.Member
{
    //訂單明細頁中的「每一個商品」
    public class VMMyOrderItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!; // 快照
        public int Price { get; set; }                   // 快照（整數）
        public int Quantity { get; set; }

        public string CreatorName { get; set; } = null!;
        public int SubTotal => Price * Quantity;
    }
}
