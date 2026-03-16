using CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
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
            var adminMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(adminMemberId))
                return Unauthorized();

            await _reviewService.ApproveAsync(id, adminMemberId, reviewNote);
            TempData["Success"] = "已通過創作者申請。";
            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reviewNote)
        {
            var adminMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(adminMemberId))
                return Unauthorized();

            await _reviewService.RejectAsync(id, adminMemberId, reviewNote);
            TempData["Warning"] = "已駁回創作者申請。";
            return RedirectToAction(nameof(Detail), new { id });
        }

        // 下一筆待審核（提高效率，不用回 Index）
        [HttpGet]
        public async Task<IActionResult> Next(int id)
        {
            var adminMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(adminMemberId))
                return Unauthorized();

            var nextId = await _reviewService.GetNextPendingIdAsync(id, adminMemberId);

            if (nextId.HasValue)
                return RedirectToAction(nameof(Detail), new { id = nextId.Value });

            return RedirectToAction(nameof(Index));
        }
    }
}