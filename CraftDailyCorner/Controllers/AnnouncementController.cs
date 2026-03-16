using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public async Task<IActionResult> Index()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var isCreator = User.IsInRole("02");
            var isAdmin = User.IsInRole("03") || User.IsInRole("04");

            var vm = await _announcementService.GetListAsync(memberId, isCreator, isAdmin);
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var isCreator = User.IsInRole("02");
            var isAdmin = User.IsInRole("03") || User.IsInRole("04");

            var vm = await _announcementService.GetDetailAsync(id, memberId, isCreator, isAdmin);
            if (vm == null) return NotFound();

            return View(vm);
        }
    }
}