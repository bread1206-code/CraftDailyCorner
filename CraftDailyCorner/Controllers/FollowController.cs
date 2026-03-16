using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers.Front
{
    [Authorize]
    public class FollowController : Controller
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(string creatorId)
        {
            var memberId = User.GetMemberId();
            if (memberId == null)
                return Unauthorized();

            var currentCreatorId = User.GetCreatorId();

            await _followService.ToggleAsync(creatorId, memberId, currentCreatorId);

            var isFollowing = await _followService.IsFollowingAsync(creatorId, memberId);
            var followerCount = await _followService.GetFollowerCountAsync(creatorId);

            // AJAX：回 JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { isFollowing, followerCount });
            }

            // 非 AJAX：維持原本導回上一頁
            return Redirect(Request.Headers.Referer.ToString());
        }
    }
}