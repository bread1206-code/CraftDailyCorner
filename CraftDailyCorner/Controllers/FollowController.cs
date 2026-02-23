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

            await _followService.ToggleAsync(creatorId, memberId);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}