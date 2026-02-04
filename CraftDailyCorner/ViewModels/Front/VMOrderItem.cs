namespace CraftDailyCorner.ViewModels.Front
{
    //訂單商品列
    public class VMOrderItem
    {
        public string ProductName { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public int SubTotal => Price * Quantity;
    }
}
