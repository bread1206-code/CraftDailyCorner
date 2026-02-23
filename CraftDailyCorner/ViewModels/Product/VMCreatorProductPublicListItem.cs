namespace CraftDailyCorner.ViewModels.Product
{
    public class VMCreatorProductPublicListItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int PriceInt => (int)Math.Floor(Price);
        public DateTime CreatedAt { get; set; }
    }
}
