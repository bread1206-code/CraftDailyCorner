using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    //分析儀表板
    [Authorize(Roles = "02")]
    public class CreatorAnalyticsController : Controller
    {
        private readonly ICreatorAnalyticsService _analyticsService;

        public CreatorAnalyticsController(ICreatorAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Community()
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var vm = await _analyticsService.GetCommunityDashboardAsync(creatorId);

            return View(vm);
        }

        public async Task<IActionResult> Commerce()
        {
            var creatorId = User.FindFirst("CreatorID")?.Value;
            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var vm = await _analyticsService.GetCommerceDashboardAsync(creatorId);
            return View(vm);
        }
    }
}