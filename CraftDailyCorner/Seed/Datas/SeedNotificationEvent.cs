using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedNotificationEvent
    {
        private readonly CraftDailyCornerContext _context;

        public SeedNotificationEvent(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.NotificationEvent.Any()) // 避免重複 Seed
            {
                var notificationEvents = new List<NotificationEvent>
                {
                    new NotificationEvent
                    {
                        NotificationType = (NotificationType)1,
                        Content = "您的訂單已成立",
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002"
                    }
                };
                _context.NotificationEvent.AddRange(notificationEvents);
                _context.SaveChanges();
            }
        }
    }
}
