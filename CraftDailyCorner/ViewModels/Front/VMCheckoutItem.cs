namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCheckoutItem
    {
        public VMProductSnapshot Product { get; set; } = null!;
        public int Quantity { get; set; }

        public decimal SubTotal => Math.Floor(Product.Price * Quantity);
    }
}
