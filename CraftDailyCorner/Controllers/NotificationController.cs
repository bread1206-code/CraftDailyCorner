using CraftDailyCorner.Extensions;
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

        public NotificationController(INotificationPreferenceService notificationPreferenceService)
        {
            _notificationPreferenceService = notificationPreferenceService;
        }

        // GET: /Notification/Preferences
        [HttpGet]
        public IActionResult Preferences()
        {
            var memberId = User.GetMemberId();
            var vm = _notificationPreferenceService.GetPreference(memberId);

            return View(vm);
        }

        // POST: /Notification/Preferences
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Preferences(VMNotificationPreference vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = User.GetMemberId();

            _notificationPreferenceService.UpdatePreference(memberId, vm);

            TempData["Success"] = "通知設定已更新";
            return RedirectToAction(nameof(Preferences));
        }
    }
}