using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.ReportCommentRe;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class PostCommentController : Controller
    {
        private readonly ICreatorPostCommentService _creatorPostCommentService;

        public PostCommentController(
            ICreatorPostCommentService creatorPostCommentService)
        {
            _creatorPostCommentService = creatorPostCommentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePostCommentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("留言資料錯誤");

            var vm = await _creatorPostCommentService.CreateAsync(
                dto,
                User.GetMemberId(),
                User.IsInRole("02") ? User.GetCreatorId() : null);

            return PartialView(
                "~/Views/Shared/_PostCommentItem.cshtml",
                vm);
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportAsync(ReportPostCommentDTO dto)
        {
            var memberId = User.GetMemberId();

            var result = await _creatorPostCommentService
                .ReportAsync(dto, memberId);

            switch (result.Result)
            {
                case ReportCommentResult.Success:
                    TempData["Success"] = "檢舉已送出";
                    break;

                case ReportCommentResult.AlreadyReported:
                    TempData["Warning"] = "您已檢舉過此留言";
                    break;

                case ReportCommentResult.Forbidden:
                    return Forbid();

                case ReportCommentResult.NotFound:
                    return NotFound();
            }

            return RedirectToAction(
                "Detail",
                "Post",
                new { id = result.PostId });
        }

        
    }
}