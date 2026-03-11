using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Notification
{
    public class VMNotificationItem
    {
        public long EventID { get; set; }

        public NotificationType NotificationType { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? LinkUrl { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}