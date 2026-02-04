using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
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
