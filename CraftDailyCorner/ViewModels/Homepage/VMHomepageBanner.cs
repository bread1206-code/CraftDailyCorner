namespace CraftDailyCorner.ViewModels.Homepage
{
    public class VMHomepageBanner
    {
        public int AutoplaySeconds { get; set; } = 5;

        public List<VMHomepageBannerItem> Items { get; set; } = new();
    }

    public class VMHomepageBannerItem
    {
        public string Title { get; set; } = null!;

        public string? Subtitle { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}