using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class CreateNotificationDTO
    {
        public string MemberID { get; set; } = null!;

        public NotificationType NotificationType { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? LinkUrl { get; set; }

        public string? RelatedEntityType { get; set; }

        public string? RelatedEntityId { get; set; }
    }
}