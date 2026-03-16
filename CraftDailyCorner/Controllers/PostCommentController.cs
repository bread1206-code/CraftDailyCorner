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

            var memberId = User.GetMemberId();
            if (memberId == null)
                return Unauthorized();

            string? creatorId = null;

            if (User.IsInRole("02"))
            {
                creatorId = User.GetCreatorId();
            }

            var vm = await _creatorPostCommentService.CreateAsync(
                dto,
                memberId,
                creatorId);

            return PartialView(
                "~/Views/Shared/_PostCommentItem.cshtml",
                vm);
        }
    }
}