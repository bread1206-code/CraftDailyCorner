namespace CraftDailyCorner.Areas.Admin.ViewModels.HomepageBanner
{
    public class VMAdminHomepageBannerIndexItem
    {
        public int BannerID { get; set; }

        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }

        public string ImageUrl { get; set; } = null!;

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? CreatedByName { get; set; }
    }
}