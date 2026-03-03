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

        public async Task<IActionResult> Index()
        {
            var vm = await _reviewService.GetIndexAsync();
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
            TempData["Success"] = "已通過創作者申請，並完成建立 CreatorProfile / 掛上 Creator 角色。";
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
    }
}