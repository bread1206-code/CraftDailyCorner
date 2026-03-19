using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationPreferenceService _notificationPreferenceService;
        private readonly INotificationService _notificationService;

        public NotificationController(
            INotificationPreferenceService notificationPreferenceService,
            INotificationService notificationService)
        {
            _notificationPreferenceService = notificationPreferenceService;
            _notificationService = notificationService;
        }

        // GET: /Notification
        [HttpGet]
        public async Task<IActionResult> Index(
            int page = 1,
            bool unreadOnly = false,
            NotificationFilterType filterType = NotificationFilterType.All)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var vm = await _notificationService.GetPagedAsync(memberId, page, 10, unreadOnly, filterType);

            return View(vm);
        }

        // POST: /Notification/MarkAsRead/5
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var ok = await _notificationService.MarkAsReadAsync(id, memberId);

            if (!ok)
                return NotFound();

            return Ok();
        }

        // POST: /Notification/MarkAllAsRead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            await _notificationService.MarkAllAsReadAsync(memberId);

            TempData["NotificationSuccess"] = "已將所有通知標記為已讀";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Notification/Preferences
        [HttpGet]
        public IActionResult Preferences()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var vm = _notificationPreferenceService.GetPreference(memberId);

            return View(vm);
        }

        // POST: /Notification/Preferences
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Preferences(VMNotificationPreference vm)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return View(vm);

            _notificationPreferenceService.UpdatePreference(memberId, vm);

            TempData["PreferencesSuccess"] = "通知設定已更新";
            return RedirectToAction(nameof(Preferences));
        }
    }
}