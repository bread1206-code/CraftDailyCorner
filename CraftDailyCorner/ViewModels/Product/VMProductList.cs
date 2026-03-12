using CraftDailyCorner.ViewModels.Breadcrumb;

namespace CraftDailyCorner.ViewModels.Product
{
    public class VMProductList
    {
        public List<VMProductListItem> Products { get; set; } = new();

        // 條件回填用
        public int? CategoryId { get; set; }
        public string? Keyword { get; set; }
        public int? TagId { get; set; }

        // 頁面標題
        public string PageTitle { get; set; } = "所有商品";

        // 麵包屑導航
        public VMBreadcrumb Breadcrumb { get; set; } = new();

        // 分頁
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}