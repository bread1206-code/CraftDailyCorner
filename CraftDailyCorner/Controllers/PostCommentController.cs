using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class PostCommentController : Controller
    {
        private readonly ICreatorPostCommentService _service;

        public PostCommentController(
            ICreatorPostCommentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePostCommentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("留言資料錯誤");

            var vm = await _service.CreateAsync(
                dto,
                User.GetMemberId(),
                User.IsInRole("02") ? User.GetCreatorId() : null);

            return PartialView(
                "~/Views/Shared/_PostCommentItem.cshtml",
                vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(
                id,
                User.GetMemberId(),
                User.IsInRole("02") ? User.GetCreatorId() : null);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Report(
            [FromBody] ReportPostCommentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("檢舉資料錯誤");

            await _service.ReportAsync(dto, User.GetMemberId());
            return Ok();
        }
    }
}