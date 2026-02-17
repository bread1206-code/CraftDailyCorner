namespace CraftDailyCorner.ImageManagementCore.ViewModels
{
    public class VMImageManagement
    {
        public string EntityId { get; set; } = null!;

        public string EntityType { get; set; } = null!;
        // "Product" / "Portfolio"
        public int? MaxImageCount { get; set; }
        public string? HintMessage { get; set; }
        public List<VMImageItem> Images { get; set; }
            = new List<VMImageItem>();
    }
}