using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMProductDetail
    {
        // ===== 商品基本資訊 =====
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // 價格（顯示用）
        public decimal Price { get; set; }
        public string DisplayPrice => Math.Floor(Price).ToString("N0");

        // 庫存
        public short StockQty { get; set; }
        public short AlertQty { get; set; }
        public bool IsOutOfStock => StockQty <= 0;
        public bool IsLowStock => StockQty > 0 && StockQty <= AlertQty;

        // ===== 圖片 =====
        public List<string> ImageUrls { get; set; } = new();

        // ===== 創作者 =====
        public string? CreatorName { get; set; }
        public string? CreatorId { get; set; }

        // ===== 分類 / Tag =====
        public List<Category> Categories { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();

        // ===== 使用者狀態 =====
        public bool IsFavorite { get; set; }

        // ===== UI 行為 =====
        public int MaxPurchaseQty => StockQty;   // 給 input max 用
        public int DefaultQty => IsOutOfStock ? 0 : 1;
    }

    }
