using CraftDailyCorner.Areas.Admin.ViewModels.Violation;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03")]
    public class ViolationController : Controller
    {
        private readonly IAdminViolationService _service;

        public ViolationController(IAdminViolationService service)
        {
            _service = service;
        }

        // mode:
        // - pending：待審核
        // - history：歷史資料 + MemberID 搜尋
        public async Task<IActionResult> Index(string mode = "pending", string? memberId = null, int page = 1)
        {
            var vm = await _service.GetIndexAsync(mode, memberId, page);

            if (vm.Mode == "history")
                return View("History", vm);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var vm = await _service.GetDetailAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkViolation(int id, string? adminNote)
        {
            try
            {
                await _service.MarkViolationAsync(id, User.GetMemberId(), adminNote);
                TempData["Danger"] = "已判定為違規，並已更新目標狀態。";
            }
            catch (ValidationException ex)
            {
                TempData["Warning"] = ex.Message;
            }
            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkNormal(int id, string? adminNote)
        {
            try
            {
                await _service.MarkNormalAsync(id, User.GetMemberId(), adminNote);
                TempData["Success"] = "已判定為正常（無違規）。";
            }
            catch (ValidationException ex)
            {
                TempData["Warning"] = ex.Message;
            }
            return RedirectToAction(nameof(Detail), new { id });
        }

        // 下一筆待審核（提高效率，不用回 Index）
        [HttpGet]
        public async Task<IActionResult> Next(long id)
        {
            var nextId = await _service.GetNextPendingIdAsync(id, User.GetMemberId());

            if (nextId.HasValue)
                return RedirectToAction(nameof(Detail), new { id = nextId.Value });

            return RedirectToAction(nameof(Index));
        }
    }
}