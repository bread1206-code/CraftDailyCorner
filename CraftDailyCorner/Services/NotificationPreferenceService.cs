using CraftDailyCorner.Models;
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

            // 若資料不存在，先補齊預設值
            EnsureDefaultPreferences(memberId, preferences);

            preferences = _context.NotificationPreferences
                .Where(x => x.MemberID == memberId)
                .ToList();

            return new VMNotificationPreference
            {
                ProductNotificationEnabled = preferences
                    .FirstOrDefault(x => x.NotificationType == NotificationType.Product)?.IsActive ?? true,

                OrderNotificationEnabled = preferences
                    .FirstOrDefault(x => x.NotificationType == NotificationType.Order)?.IsActive ?? true,

                CreatorNotificationEnabled = preferences
                    .FirstOrDefault(x => x.NotificationType == NotificationType.CreatorPost)?.IsActive ?? true
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

            UpdateSinglePreference(preferences, memberId, NotificationType.Product, vm.ProductNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.Order, vm.OrderNotificationEnabled);
            UpdateSinglePreference(preferences, memberId, NotificationType.CreatorPost, vm.CreatorNotificationEnabled);

            _context.SaveChanges();
        }

        private void EnsureDefaultPreferences(string memberId, List<NotificationPreference> existingPreferences)
        {
            var now = DateTime.Now;
            bool changed = false;

            if (!existingPreferences.Any(x => x.NotificationType == NotificationType.Product))
            {
                _context.NotificationPreferences.Add(new NotificationPreference
                {
                    MemberID = memberId,
                    NotificationType = NotificationType.Product,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                changed = true;
            }

            if (!existingPreferences.Any(x => x.NotificationType == NotificationType.Order))
            {
                _context.NotificationPreferences.Add(new NotificationPreference
                {
                    MemberID = memberId,
                    NotificationType = NotificationType.Order,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                changed = true;
            }

            if (!existingPreferences.Any(x => x.NotificationType == NotificationType.CreatorPost))
            {
                _context.NotificationPreferences.Add(new NotificationPreference
                {
                    MemberID = memberId,
                    NotificationType = NotificationType.CreatorPost,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                changed = true;
            }

            if (changed)
                _context.SaveChanges();
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