namespace CraftDailyCorner.ImageManagementCore.ViewModels
{
    public class VMImageItem
    {
        public long ImageId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int SortOrder { get; set; }
    }
}