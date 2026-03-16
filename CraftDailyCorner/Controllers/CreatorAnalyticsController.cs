using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    // 分析儀表板
    [Authorize(Roles = "02")]
    public class CreatorAnalyticsController : Controller
    {
        private readonly ICreatorAnalyticsService _analyticsService;

        public CreatorAnalyticsController(ICreatorAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // 社群分析儀表板
        [HttpGet]
        public async Task<IActionResult> Community()
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var vm = await _analyticsService.GetCommunityDashboardAsync(creatorId);

            return View(vm);
        }

        // 商務分析儀表板
        [HttpGet]
        public async Task<IActionResult> Commerce()
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var vm = await _analyticsService.GetCommerceDashboardAsync(creatorId);
            return View(vm);
        }

        // =========================
        // Commerce AJAX APIs
        // =========================

        [HttpGet]
        public async Task<IActionResult> GetCommerceRevenueTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommerceRevenueTrendAsync(creatorId, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommerceOrderTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommerceOrderTrendAsync(creatorId, query);
            return Json(result);
        }

        // =========================
        // Community AJAX APIs
        // =========================

        [HttpGet]
        public async Task<IActionResult> GetCommunityPostTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommunityPostTrendAsync(creatorId, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommunityPortfolioTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommunityPortfolioTrendAsync(creatorId, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommunityCommentTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommunityCommentTrendAsync(creatorId, query);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommunityReactionTrend([FromQuery] VMAnalyticsChartQuery query)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized();

            var result = await _analyticsService.GetCommunityReactionTrendAsync(creatorId, query);
            return Json(result);
        }
    }
}