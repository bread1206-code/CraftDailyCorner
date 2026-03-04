using CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03")]
    public class CreatorReviewController : Controller
    {
        private readonly IAdminCreatorReviewService _reviewService;

        public CreatorReviewController(IAdminCreatorReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // mode:
        // - pending：待審核
        // - history：歷史資料 + MemberID 搜尋
        public async Task<IActionResult> Index(string mode = "pending", string? memberId = null, int page = 1)
        {
            var vm = await _reviewService.GetIndexAsync(mode, memberId, page);

            if (vm.Mode == "history")
                return View("History", vm);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var vm = await _reviewService.GetDetailAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string? reviewNote)
        {
            await _reviewService.ApproveAsync(id, User.GetMemberId(), reviewNote);
            TempData["Success"] = "已通過創作者申請。";
            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reviewNote)
        {
            await _reviewService.RejectAsync(id, User.GetMemberId(), reviewNote);
            TempData["Warning"] = "已駁回創作者申請。";
            return RedirectToAction(nameof(Detail), new { id });
        }

        //新增：下一筆待審核（提高效率，不用回 Index）
        [HttpGet]
        public async Task<IActionResult> Next(int id)
        {
            var adminMemberId = User.GetMemberId();

            var nextId = await _reviewService.GetNextPendingIdAsync(id, adminMemberId);

            if (nextId.HasValue)
                return RedirectToAction(nameof(Detail), new { id = nextId.Value });

            // 沒有下一筆 -> 回列表
            return RedirectToAction(nameof(Index));
        }
    }
}