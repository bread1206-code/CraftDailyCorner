using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
    //商品列表頁使用，每一筆商品卡片需要的資料集合
    public class VMProductList
    {
        public List<Product> Products { get; set; } = new();
        public List<ProductImage> Images { get; set; } = null!;

        // 條件回填用（之後前端很好做）
        public int? CategoryId { get; set; }
        public string? Keyword { get; set; }
        public int? TagId { get; set; }
    }
}
