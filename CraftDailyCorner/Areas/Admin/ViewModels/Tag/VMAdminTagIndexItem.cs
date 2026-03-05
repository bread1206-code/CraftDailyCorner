namespace CraftDailyCorner.Areas.Admin.ViewModels.Tag
{
    public class VMAdminTagIndexItem
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}