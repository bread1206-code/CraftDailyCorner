namespace CraftDailyCorner.ViewModels.Announcement
{
    public class VMAnnouncementDetail
    {
        public int AnnouncementID { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}