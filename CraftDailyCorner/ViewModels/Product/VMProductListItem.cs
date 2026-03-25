namespace CraftDailyCorner.ViewModels.Product
{
    public class VMProductListItem
    {
        public string ProductID { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? CreatorID { get; set; }

        public bool IsFavorite { get; set; }

        public string CoverImagePath =>
            string.IsNullOrWhiteSpace(CoverImageUrl) || string.IsNullOrWhiteSpace(CreatorID)
                ? "/images/no-image.webp"
                : $"/Photos/04ProductImage/{CreatorID}/Large/{CoverImageUrl}.webp";
    }
}