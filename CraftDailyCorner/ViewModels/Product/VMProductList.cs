using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Breadcrumb;

namespace CraftDailyCorner.ViewModels.Product
{
    //商品列表頁使用，每一筆商品卡片需要的資料集合
    public class VMProductList
    {
        public List<VMProductListItem> Products { get; set; } = new();

        // 條件回填用
        public int? CategoryId { get; set; }
        public string? Keyword { get; set; }
        public int? TagId { get; set; }
        // 新增：頁面標題
        public string PageTitle { get; set; } = "所有商品";
        // 新增：麵包屑導航
        public VMBreadcrumb Breadcrumb { get; set; } = new();
    }
}
