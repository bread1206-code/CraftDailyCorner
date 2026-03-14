using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Breadcrumb;

namespace CraftDailyCorner.ViewModels.Product
{
    // 商品詳細頁使用，包含完整商品資訊
    public class VMProductDetail
    {
        // 商品基本資訊
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // 價格（顯示用）
        public decimal Price { get; set; }
        public string DisplayPrice => Math.Floor(Price).ToString("N0");

        // 庫存
        public int StockQty { get; set; }
        public int AlertQty { get; set; }
        public bool IsOutOfStock => StockQty <= 0;
        public bool IsLowStock => StockQty > 0 && StockQty <= AlertQty;

        // 圖片
        public List<string> ImageUrls { get; set; } = new();

        // 創作者
        public string? CreatorName { get; set; }
        public string? CreatorID { get; set; }

        // 分類 / Tag
        public List<Category> Categories { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();

        // 使用者狀態
        public bool IsFavorite { get; set; }
        public bool IsOwner { get; set; }

        // UI 行為
        public int MaxPurchaseQty => StockQty;
        public int DefaultQty => IsOutOfStock ? 0 : 1;

        // 麵包屑導航
        public VMBreadcrumb Breadcrumb { get; set; } = new();

        // 檢舉相關
        public bool IsReportBanned { get; set; }
        public DateTime? ReportBanUntil { get; set; }

        // 圖片路徑
        public string GetLargeImagePath(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || string.IsNullOrWhiteSpace(CreatorID))
                return "/images/no-image.png";

            return $"/Photos/04ProductImage/{CreatorID}/Large/{imageUrl}.png";
        }

        public string GetMediumImagePath(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || string.IsNullOrWhiteSpace(CreatorID))
                return "/images/no-image.png";

            return $"/Photos/04ProductImage/{CreatorID}/Medium/{imageUrl}.png";
        }
    }
}