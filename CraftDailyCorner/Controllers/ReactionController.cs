using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Route("reaction")]
    public class ReactionController : Controller
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle(
            [FromForm] ReactionTargetType targetType,
            [FromForm] string targetId,
            [FromForm] ReactionType reactionType)
        {
            if (!User.Identity!.IsAuthenticated)
                return Unauthorized();

            var memberId = User.GetMemberId();

            if (memberId == null)
                return Unauthorized();

            var result = await _reactionService
                .ToggleAsync(memberId, targetType, targetId, reactionType);

            return Json(result);
        }
    }
}
