using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedNotificationPreferences
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedNotificationPreferences(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            var existingPreferences = _context.NotificationPreferences
                .Select(x => new { x.MemberID, x.NotificationType })
                .ToHashSet();

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

            var preferences = new List<NotificationPreference>();

            foreach (var row in seedContext.Members)
            {
                foreach (var type in defaultTypes)
                {
                    if (existingPreferences.Contains(new { row.MemberID, NotificationType = type }))
                        continue;

                    preferences.Add(new NotificationPreference
                    {
                        MemberID = row.MemberID,
                        NotificationType = type,
                        IsActive = true,
                        CreatedAt = row.CreatedAt,
                        UpdatedAt = row.CreatedAt
                    });
                }
            }

            if (preferences.Any())
            {
                _context.NotificationPreferences.AddRange(preferences);
                _context.SaveChanges();
            }
        }
    }
}