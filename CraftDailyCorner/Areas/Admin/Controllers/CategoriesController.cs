using CraftDailyCorner.Areas.Admin.ViewModels.Category;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class CategoriesController : Controller
    {
        private readonly IAdminCategoryService _service;

        public CategoriesController(IAdminCategoryService service)
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
        public async Task<IActionResult> Create(VMAdminCategoryUpsert vm)
        {
            if (!ModelState.IsValid)
            {
                var refill = await _service.GetCreateVmAsync();
                vm.ParentCategoryOptions = refill.ParentCategoryOptions;
                return View(vm);
            }

            await _service.CreateAsync(vm);
            TempData["Success"] = "分類已新增";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetEditVmAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMAdminCategoryUpsert vm)
        {
            if (!ModelState.IsValid)
            {
                if (vm.CategoryID != null)
                {
                    var refill = await _service.GetEditVmAsync(vm.CategoryID.Value);
                    if (refill != null) vm.ParentCategoryOptions = refill.ParentCategoryOptions;
                }
                return View(vm);
            }

            var ok = await _service.UpdateAsync(vm);
            if (!ok) return NotFound();

            TempData["Success"] = "分類已更新";
            return RedirectToAction(nameof(Index));
        }

        // 軟刪除：停用
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(int id)
        {
            var (ok, message) = await _service.DisableAsync(id);

            if (!ok)
            {
                TempData["Warning"] = message ?? "停用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "分類已停用";
            return RedirectToAction(nameof(Index));
        }

        // 復原：啟用
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enable(int id)
        {
            var (ok, message) = await _service.EnableAsync(id);

            if (!ok)
            {
                TempData["Warning"] = message ?? "啟用失敗";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "分類已啟用";
            return RedirectToAction(nameof(Index));
        }
    }
}