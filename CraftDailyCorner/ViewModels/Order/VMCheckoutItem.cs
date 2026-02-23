namespace CraftDailyCorner.ViewModels.Order
{
    //結帳商品
    public class VMCheckoutItem
    {
        public int Quantity { get; set; }

        public VMProductSnapshot Product { get; set; } = null!;
    }
}
