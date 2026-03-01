namespace CraftDailyCorner.ViewModels.Product
{
    public class VMCreatorProductPublicListItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        //  Profile.cshtml 會用到
        public bool IsFavorite { get; set; }

        // ===== UI Helper（可選）=====
        public int PriceInt => (int)Math.Floor(Price);

        public string CoverImagePath =>
            $"/Photos/04ProductImage/Large/{ImageUrl}.png";
    }
}