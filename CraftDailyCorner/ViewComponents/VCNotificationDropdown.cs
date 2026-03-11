using Microsoft.AspNetCore.Mvc;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Notification;
using CraftDailyCorner.Extensions;

namespace CraftDailyCorner.ViewComponents
{
    public class VCNotificationDropdown : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public VCNotificationDropdown(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var memberId = UserClaimsPrincipal?.GetMemberId();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                return View(new VMNotificationDropdown());
            }

            var unreadCount = await _notificationService.GetUnreadCountAsync(memberId);

            var items = await _notificationService.GetRecentAsync(memberId, 5);

            var vm = new VMNotificationDropdown
            {
                UnreadCount = unreadCount,
                Items = items
            };

            return View(vm);
        }
    }
}