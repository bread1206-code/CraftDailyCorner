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
            if (_context.NotificationEvents.Any())
                return;

            var now = DateTime.Now;

            var notificationEvents = new List<NotificationEvent>
            {
                new NotificationEvent
                {
                    NotificationType = NotificationType.OrderCreated,
                    Title = "訂單已成立",
                    Content = "您的訂單已成立，系統已開始處理。",
                    LinkUrl = "/Orders/Detail/202601010001",
                    IsRead = false,
                    ReadAt = null,
                    RelatedEntityType = "Order",
                    RelatedEntityId = "202601010001",
                    CreatedAt = now,
                    MemberID = "M0000002"
                },
                new NotificationEvent
                {
                    NotificationType = NotificationType.CreatorNewPost,
                    Title = "創作者發布了新日誌",
                    Content = "您追蹤的創作者發布了新的創作日誌。",
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
                    NotificationType = NotificationType.FavoriteProductPublished,
                    Title = "收藏商品已上架",
                    Content = "您收藏的商品現在已上架，可以前往查看。",
                    LinkUrl = "/Products/Detail/P000000001",
                    IsRead = false,
                    ReadAt = null,
                    RelatedEntityType = "Product",
                    RelatedEntityId = "P000000001",
                    CreatedAt = now,
                    MemberID = "M0000002"
                }
            };

            _context.NotificationEvents.AddRange(notificationEvents);
            _context.SaveChanges();
        }
    }
}