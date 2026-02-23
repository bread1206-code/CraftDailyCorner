namespace CraftDailyCorner.ViewModels.CreatorPickList
{
    // 撿貨單中的商品明細
    public class VMPickListOrderItem
    {
        public string ProductID { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
