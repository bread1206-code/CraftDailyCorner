namespace CraftDailyCorner.ViewModels.Member
{
    public class VMMyOrderItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
        public string CreatorID { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public string BrandName { get; set; } = null!;
        public int SubTotal => Price * Quantity;

        public string ProductImagePath =>
            string.IsNullOrWhiteSpace(ProductImage)
                ? "/images/no-image.png"
                : $"/Photos/04ProductImage/{CreatorID}/Medium/{ProductImage}.png";

        // ===== 商品評價 =====
        public bool HasReview { get; set; }
        public long? ReviewID { get; set; }
        public byte? ReviewRating { get; set; }
        public string? ReviewComment { get; set; }
    }
}