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

    }
}