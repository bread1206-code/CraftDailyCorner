namespace CraftDailyCorner.Areas.Admin.ViewModels.Category
{
    public class VMAdminCategoryIndexItem
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;

        public int? ParentCategoryID { get; set; }
        public string? ParentCategoryName { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // 0 = 大分類, 1 = 小分類（用來在 Index 做縮排）
        public int Level { get; set; }
    }
}