using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

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
            if (!_context.NotificationEvents.Any())
            {
                var now = DateTime.Now;

                var notificationEvents = new List<NotificationEvent>
                {
                    new NotificationEvent
                    {
                        NotificationType = NotificationType.Order,
                        Title = "訂單通知",
                        Content = "您的訂單已成立",
                        LinkUrl = "/Orders",
                        IsRead = false,
                        ReadAt = null,
                        RelatedEntityType = "Order",
                        RelatedEntityId = "O0000001",
                        CreatedAt = now,
                        MemberID = "M0000002"
                    },
                    new NotificationEvent
                    {
                        NotificationType = NotificationType.CreatorPost,
                        Title = "創作日誌更新",
                        Content = "您追蹤的創作者發布了新的創作日誌",
                        LinkUrl = "/Post/Detail/P0000001",
                        IsRead = false,
                        ReadAt = null,
                        RelatedEntityType = "Post",
                        RelatedEntityId = "P0000001",
                        CreatedAt = now,
                        MemberID = "M0000002"
                    },
                    new NotificationEvent
                    {
                        NotificationType = NotificationType.Product,
                        Title = "商品通知",
                        Content = "您收藏的商品已重新上架",
                        LinkUrl = "/Products/Detail/PR000001",
                        IsRead = false,
                        ReadAt = null,
                        RelatedEntityType = "Product",
                        RelatedEntityId = "PR000001",
                        CreatedAt = now,
                        MemberID = "M0000002"
                    }
                };

                _context.NotificationEvents.AddRange(notificationEvents);
                _context.SaveChanges();
            }
        }
    }
}