namespace CraftDailyCorner.ViewModels.Member
{
    public class VMMyOrderItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public string BrandName { get; set; } = null!;
        public int SubTotal => Price * Quantity;

        // ===== 商品評價 =====
        public bool HasReview { get; set; }
        public long? ReviewID { get; set; }
        public byte? ReviewRating { get; set; }
        public string? ReviewComment { get; set; }
    }
}