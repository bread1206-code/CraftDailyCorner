using CraftDailyCorner.Areas.Admin.ViewModels.HomepageBanner;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class HomepageBannerController : Controller
    {
        private readonly IAdminHomepageBannerService _service;

        public HomepageBannerController(IAdminHomepageBannerService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _service.GetIndexAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await _service.GetCreateVmAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMAdminHomepageBannerUpsert vm)
        {
            if (vm.ImageFile == null || vm.ImageFile.Length == 0)
                ModelState.AddModelError(nameof(vm.ImageFile), "請上傳輪播圖片");

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var adminId = User.GetMemberId();
                if (adminId == null)
                    return Unauthorized();

                await _service.CreateAsync(vm, adminId);

                TempData["HomepageBannerSuccess"] = "首頁輪播圖片已新增";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetEditVmAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMAdminHomepageBannerUpsert vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var ok = await _service.UpdateAsync(vm);
                if (!ok) return NotFound();

                TempData["HomepageBannerSuccess"] = "首頁輪播圖片已更新";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(int id)
        {
            var (ok, message) = await _service.DisableAsync(id);

            if (!ok)
            {
                TempData["HomepageBannerWarning"] = message ?? "停用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["HomepageBannerSuccess"] = "首頁輪播圖片已停用";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enable(int id)
        {
            var (ok, message) = await _service.EnableAsync(id);

            if (!ok)
            {
                TempData["HomepageBannerWarning"] = message ?? "啟用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["HomepageBannerSuccess"] = "首頁輪播圖片已啟用";
            return RedirectToAction(nameof(Index));
        }
    }
}