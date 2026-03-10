namespace CraftDailyCorner.ViewModels.Announcement
{
    public class VMAnnouncementList
    {
        public List<VMAnnouncementListItem> Items { get; set; } = new();
    }

    public class VMAnnouncementListItem
    {
        public int AnnouncementID { get; set; }

        public string Title { get; set; } = null!;

        public string ContentPreview { get; set; } = null!;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}