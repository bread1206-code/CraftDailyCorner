namespace CraftDailyCorner.ViewModels.Notification
{
    public class VMNotificationDropdown
    {
        public int UnreadCount { get; set; }

        public List<VMNotificationItem> Items { get; set; } = new();
    }
}