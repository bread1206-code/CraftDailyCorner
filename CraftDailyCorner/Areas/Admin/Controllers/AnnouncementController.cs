using CraftDailyCorner.Areas.Admin.ViewModels.Announcement;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class AnnouncementController : Controller
    {
        private readonly IAdminAnnouncementService _service;

        public AnnouncementController(IAdminAnnouncementService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _service.GetIndexAsync();
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var vm = await _service.GetDetailAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            var vm = await _service.GetCreateVmAsync(currentMemberId, isSuperAdmin);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMAdminAnnouncementUpsert vm)
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            if (!ModelState.IsValid)
            {
                var refill = await _service.GetCreateVmAsync(currentMemberId, isSuperAdmin);
                vm.AudienceOptions = refill.AudienceOptions;
                vm.StatusOptions = refill.StatusOptions;
                return View(vm);
            }

            try
            {
                var id = await _service.CreateAsync(vm, currentMemberId, isSuperAdmin);
                TempData["Success"] = "公告已新增";
                return RedirectToAction(nameof(Detail), new { id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var refill = await _service.GetCreateVmAsync(currentMemberId, isSuperAdmin);
                vm.AudienceOptions = refill.AudienceOptions;
                vm.StatusOptions = refill.StatusOptions;
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            var vm = await _service.GetEditVmAsync(id, currentMemberId, isSuperAdmin);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMAdminAnnouncementUpsert vm)
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            if (!ModelState.IsValid)
            {
                if (vm.AnnouncementID.HasValue)
                {
                    var refill = await _service.GetEditVmAsync(vm.AnnouncementID.Value, currentMemberId, isSuperAdmin);
                    if (refill != null)
                    {
                        vm.AudienceOptions = refill.AudienceOptions;
                        vm.StatusOptions = refill.StatusOptions;
                    }
                }
                return View(vm);
            }

            try
            {
                var ok = await _service.UpdateAsync(vm, currentMemberId, isSuperAdmin);
                if (!ok) return NotFound();

                TempData["Success"] = "公告已更新";
                return RedirectToAction(nameof(Detail), new { id = vm.AnnouncementID });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                if (vm.AnnouncementID.HasValue)
                {
                    var refill = await _service.GetEditVmAsync(vm.AnnouncementID.Value, currentMemberId, isSuperAdmin);
                    if (refill != null)
                    {
                        vm.AudienceOptions = refill.AudienceOptions;
                        vm.StatusOptions = refill.StatusOptions;
                    }
                }

                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            var (ok, message) = await _service.ActivateAsync(id, currentMemberId, isSuperAdmin);

            if (!ok)
            {
                TempData["Warning"] = message ?? "啟用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "公告已啟用";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inactivate(int id)
        {
            var currentMemberId = User.GetMemberId();
            var isSuperAdmin = User.IsInRole("04");

            var (ok, message) = await _service.InactivateAsync(id, currentMemberId, isSuperAdmin);

            if (!ok)
            {
                TempData["Warning"] = message ?? "停用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "公告已停用";
            return RedirectToAction(nameof(Index));
        }
    }
}