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
            if (!_context.NotificationPreferences.Any()) // 避免重複 Seed
            {
                var notificationPreferences = new List<NotificationPreference>
                {
                    new NotificationPreference
                    {
                        NotificationType = (NotificationType)1,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        MemberID = "M0000002"
                    }
                };
                _context.NotificationPreferences.AddRange(notificationPreferences);
                _context.SaveChanges();
            }
        }
    }
}
