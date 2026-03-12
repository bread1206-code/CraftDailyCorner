namespace CraftDailyCorner.ViewModels.Member
{
    public class VMFavoriteProductItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string CreatorID { get; set; } = null!;
        public decimal Price { get; set; }
        public string CoverImageUrl { get; set; } = "no-image.png";
        public bool IsFavorite { get; set; } = true; // 一定是 true
    }
}
