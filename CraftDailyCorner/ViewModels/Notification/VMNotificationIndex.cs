using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Notification
{
    public class VMNotificationIndex
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public int UnreadCount { get; set; }

        public bool UnreadOnly { get; set; }

        public NotificationFilterType FilterType { get; set; } = NotificationFilterType.All;

        public List<VMNotificationItem> Items { get; set; } = new();
    }
}