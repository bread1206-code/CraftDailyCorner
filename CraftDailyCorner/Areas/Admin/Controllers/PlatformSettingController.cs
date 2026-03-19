using CraftDailyCorner.Areas.Admin.ViewModels.PlatformSetting;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class PlatformSettingController : Controller
    {
        private readonly IAdminPlatformSettingService _service;

        public PlatformSettingController(IAdminPlatformSettingService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _service.GetIndexAsync();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetEditAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMAdminPlatformSettingEdit vm)
        {
            var refill = await _service.GetEditAsync(vm.SettingID);

            if (!ModelState.IsValid)
            {
                if (refill != null)
                {
                    vm.SettingKey = refill.SettingKey;
                    vm.DataType = refill.DataType;
                    vm.CategoryID = refill.CategoryID;
                    vm.CategoryName = refill.CategoryName;
                    vm.Description = refill.Description;
                    vm.UpdatedAt = refill.UpdatedAt;
                    vm.UpdatedBy = refill.UpdatedBy;
                    vm.UpdatedByName = refill.UpdatedByName;
                    vm.BoolOptions = refill.BoolOptions;
                    vm.HintText = refill.HintText;
                    vm.SuggestedRange = refill.SuggestedRange;
                }

                return View(vm);
            }

            try
            {
                var adminId = User.GetMemberId();
                if (adminId == null)
                    return Unauthorized();

                var ok = await _service.UpdateAsync(vm, adminId);
                if (!ok) return NotFound();

                TempData["PlatformSettingSuccess"] = "平台參數已更新";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.SettingValue), ex.Message);

                if (refill != null)
                {
                    vm.SettingKey = refill.SettingKey;
                    vm.DataType = refill.DataType;
                    vm.CategoryID = refill.CategoryID;
                    vm.CategoryName = refill.CategoryName;
                    vm.Description = refill.Description;
                    vm.UpdatedAt = refill.UpdatedAt;
                    vm.UpdatedBy = refill.UpdatedBy;
                    vm.UpdatedByName = refill.UpdatedByName;
                    vm.BoolOptions = refill.BoolOptions;
                    vm.HintText = refill.HintText;
                    vm.SuggestedRange = refill.SuggestedRange;
                }

                return View(vm);
            }
        }
    }
}