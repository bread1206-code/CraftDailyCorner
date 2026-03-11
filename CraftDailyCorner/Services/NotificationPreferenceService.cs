using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Notification;

namespace CraftDailyCorner.Services
{
    public class NotificationPreferenceService : INotificationPreferenceService
    {
        private readonly CraftDailyCornerContext _context;

        public NotificationPreferenceService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public VMNotificationPreference GetPreference(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            var preferences = _context.NotificationPreferences
                .Where(x => x.MemberID == memberId)
                .ToList();

            EnsureDefaultPreferences(memberId, preferences);

            preferences = _context.NotificationPreferences
                .Where(x => x.MemberID == memberId)
                .ToList();

            return new VMNotificationPreference
            {
                // 商品通知群組：收藏商品上架 / 補貨
                ProductNotificationEnabled =
                    IsGroupEnabled(preferences, NotificationType.FavoriteProductPublished) &&
                    IsGroupEnabled(preferences, NotificationType.FavoriteProductRestocked),

                // 訂單通知群組：會員訂單 + 創作者訂單完成
                OrderNotificationEnabled =
                    IsGroupEnabled(preferences, NotificationType.OrderCreated) &&
                    IsGroupEnabled(preferences, NotificationType.OrderPaid) &&
                    IsGroupEnabled(preferences, NotificationType.OrderShipped) &&
                    IsGroupEnabled(preferences, NotificationType.OrderDelivered) &&
                    IsGroupEnabled(preferences, NotificationType.OrderCompleted),

                // 創作者通知群組：追蹤創作者 + 低庫存 + 缺貨 + 留言
                CreatorNotificationEnabled =
                    IsGroupEnabled(preferences, NotificationType.CreatorNewPost) &&
                    IsGroupEnabled(preferences, NotificationType.CreatorNewProduct) &&
                    IsGroupEnabled(preferences, NotificationType.CreatorNewPortfolio) &&
                    IsGroupEnabled(preferences, NotificationType.ProductLowStock) &&
                    IsGroupEnabled(preferences, NotificationType.ProductOutOfStock) &&
                    IsGroupEnabled(preferences, NotificationType.PostComment)
            };
        }

        public void UpdatePreference(string memberId, VMNotificationPreference vm)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            var preferences = _context.NotificationPreferences
                .Where(x => x.MemberID == memberId)
                .ToList();

            EnsureDefaultPreferences(memberId, preferences);

            preferences = _context.NotificationPreferences
                .Where(x => x.MemberID == memberId)
                .ToList();

            // ===== 商品通知群組 =====
            UpdateSinglePreference(preferences, memberId, NotificationType.FavoriteProductPublished, vm.ProductNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.FavoriteProductRestocked, vm.ProductNotificationEnabled);

            // ===== 訂單通知群組 =====
            UpdateSinglePreference(preferences, memberId, NotificationType.OrderCreated, vm.OrderNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.OrderPaid, vm.OrderNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.OrderShipped, vm.OrderNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.OrderDelivered, vm.OrderNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.OrderCompleted, vm.OrderNotificationEnabled);

            // ===== 創作者通知群組 =====
            UpdateSinglePreference(preferences, memberId, NotificationType.CreatorNewPost, vm.CreatorNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.CreatorNewProduct, vm.CreatorNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.CreatorNewPortfolio, vm.CreatorNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.ProductLowStock, vm.CreatorNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.ProductOutOfStock, vm.CreatorNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.PostComment, vm.CreatorNotificationEnabled);

            // 公告預設一律存在，但目前不放在 UI 三大開關內
            UpdateSinglePreference(preferences, memberId, NotificationType.Announcement, true);

            _context.SaveChanges();
        }

        private void EnsureDefaultPreferences(string memberId, List<NotificationPreference> existingPreferences)
        {
            var now = DateTime.Now;
            bool changed = false;

            foreach (var type in GetDefaultTypes())
            {
                if (existingPreferences.Any(x => x.NotificationType == type))
                    continue;

                _context.NotificationPreferences.Add(new NotificationPreference
                {
                    MemberID = memberId,
                    NotificationType = type,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });

                changed = true;
            }

            if (changed)
                _context.SaveChanges();
        }

        private static List<NotificationType> GetDefaultTypes()
        {
            return new List<NotificationType>
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
        }

        private static bool IsGroupEnabled(List<NotificationPreference> preferences, NotificationType type)
        {
            return preferences.FirstOrDefault(x => x.NotificationType == type)?.IsActive ?? true;
        }

        private void UpdateSinglePreference(
            List<NotificationPreference> preferences,
            string memberId,
            NotificationType type,
            bool isActive)
        {
            var target = preferences.FirstOrDefault(x => x.NotificationType == type);

            if (target == null)
            {
                _context.NotificationPreferences.Add(new NotificationPreference
                {
                    MemberID = memberId,
                    NotificationType = type,
                    IsActive = isActive,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                return;
            }

            target.IsActive = isActive;
            target.UpdatedAt = DateTime.Now;
        }
    }
}