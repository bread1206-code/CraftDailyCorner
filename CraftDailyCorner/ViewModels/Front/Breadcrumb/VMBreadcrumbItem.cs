namespace CraftDailyCorner.ViewModels.Front.Breadcrumb
{
    public class VMBreadcrumbItem
    {
        public string Text { get; set; } = null!;
        public string? Url { get; set; }  // null = 目前頁，不可點
    }
}
