using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMCreatorProductList
    {
        public List<VMCreatorProductListItem> Products { get; set; } = new();
    }

    public class VMCreatorProductListItem
    {
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        public string CreatorID { get; set; } = null!;

        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "狀態")]
        public string StatusName { get; set; } = null!;
        public int StatusID { get; set; }
        [Display(Name = "庫存")]
        public int StockQty { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "封面圖片")]
        public string? CoverImageUrl { get; set; }

        [Display(Name = "警示庫存")]
        public int AlertQty { get; set; }

        [Display(Name = "缺貨")]
        public bool IsOutOfStock => StockQty == 0;

        [Display(Name = "庫存不足")]
        public bool IsLowStock => StockQty > 0 && StockQty <= AlertQty;

        public string CoverImagePath =>
            string.IsNullOrEmpty(CoverImageUrl)
                ? "/Photos/04ProductImage/default.webp"
                : $"/Photos/04ProductImage/{CreatorID}/Medium/{CoverImageUrl}.webp";
    }
}