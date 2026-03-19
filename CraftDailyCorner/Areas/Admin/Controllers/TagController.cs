using CraftDailyCorner.Areas.Admin.ViewModels.Tag;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class TagController : Controller
    {
        private readonly IAdminTagService _tagService;

        public TagController(IAdminTagService tagService)
        {
            _tagService = tagService;
        }

        // GET: /Admin/Tag
        public async Task<IActionResult> Index()
        {
            var vm = await _tagService.GetIndexAsync();
            return View(vm);
        }

        // GET: /Admin/Tag/Create
        public IActionResult Create()
        {
            return View(new VMAdminTagEdit());
        }

        // POST: /Admin/Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMAdminTagEdit vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _tagService.CreateAsync(vm);
                TempData["TagSuccess"] = "標籤新增成功";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: /Admin/Tag/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _tagService.GetEditAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        // POST: /Admin/Tag/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMAdminTagEdit vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var ok = await _tagService.UpdateAsync(vm);
                if (!ok) return NotFound();

                TempData["TagSuccess"] = "標籤更新成功";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // POST: /Admin/Tag/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            var ok = await _tagService.ToggleActiveAsync(id);
            if (!ok) return NotFound();

            TempData["TagSuccess"] = "標籤狀態已更新";
            return RedirectToAction(nameof(Index));
        }
    }
}