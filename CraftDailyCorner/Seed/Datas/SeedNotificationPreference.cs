using CraftDailyCorner.Models;

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
            if (!_context.NotificationPreferences.Any())
            {
                var now = DateTime.Now;

                var notificationPreferences = new List<NotificationPreference>
                {
                    new NotificationPreference
                    {
                        NotificationType = NotificationType.Product,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now,
                        MemberID = "M0000002"
                    },
                    new NotificationPreference
                    {
                        NotificationType = NotificationType.Order,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now,
                        MemberID = "M0000002"
                    },
                    new NotificationPreference
                    {
                        NotificationType = NotificationType.CreatorPost,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now,
                        MemberID = "M0000002"
                    }
                };

                _context.NotificationPreferences.AddRange(notificationPreferences);
                _context.SaveChanges();
            }
        }
    }
}