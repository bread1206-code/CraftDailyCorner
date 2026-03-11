using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedNotificationPreference
    {
        private readonly CraftDailyCornerContext _context;

        public SeedNotificationPreference(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.NotificationPreferences.Any())
                return;

            var now = DateTime.Now;

            var memberIds = _context.Members.Select(m => m.MemberID).ToList();

            var defaultTypes = new List<NotificationType>
            {
                NotificationType.Announcement,

                NotificationType.FavoriteProductPublished,
                NotificationType.FavoriteProductRestocked,

                NotificationType.CreatorNewPost,
                NotificationType.CreatorNewProduct,
                NotificationType.CreatorNewPortfolio,

                NotificationType.OrderCreated,
                NotificationType.OrderPaid,
                NotificationType.OrderShipped,
                NotificationType.OrderDelivered,
                NotificationType.OrderCompleted,

                NotificationType.ProductLowStock,
                NotificationType.ProductOutOfStock,
                NotificationType.PostComment
            };

            var notificationPreferences = memberIds
                .SelectMany(memberId => defaultTypes
                    .Select(type => new NotificationPreference
                    {
                        MemberID = memberId,
                        NotificationType = type,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now
                    }))
                .ToList();

            _context.NotificationPreferences.AddRange(notificationPreferences);
            _context.SaveChanges();
        }
    }
}