using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
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
    }
}