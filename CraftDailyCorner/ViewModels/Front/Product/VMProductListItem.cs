namespace CraftDailyCorner.ViewModels.Front.Product
{
    // 商品列表頁「單張卡片」專用
    public class VMProductListItem
    {
        public string ProductID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }

        // 封面圖（已處理好，View 直接用）
        public string CoverImageUrl { get; set; } = "no-image.png";

        // 收藏狀態
        public bool IsFavorite { get; set; }
    }
}
