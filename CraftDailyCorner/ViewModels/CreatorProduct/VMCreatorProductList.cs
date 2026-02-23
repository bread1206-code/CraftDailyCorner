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

        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "狀態")]
        public string StatusName { get; set; } = null!;

        [Display(Name = "庫存")]
        public int StockQty { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
    }
}