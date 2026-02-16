namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMCategoryGroup
    {
        public string ParentCategoryName { get; set; } = null!;
        public List<VMCategoryChild> Children { get; set; } = new();
    }

    public class VMCategoryChild
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}