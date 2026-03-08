using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.ViewComponents
{
    public class VCPostComment : ViewComponent
    {
        private readonly ICreatorPostCommentService _service;

        public VCPostComment(
            ICreatorPostCommentService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(string postId)
        {
            var user = HttpContext.User;

            string? memberId = null;
            string? creatorId = null;

            if (user.Identity?.IsAuthenticated == true)
            {
                memberId = user.GetMemberId();

                if (user.IsInRole("02"))
                {
                    creatorId = user.GetCreatorId();
                }
            }

            var comments = await _service
                .GetPostCommentsAsync(
                    postId,
                    memberId,
                    creatorId);

            return View(comments);
        }
    }
}