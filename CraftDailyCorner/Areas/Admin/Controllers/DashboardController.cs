using CraftDailyCorner.Areas.Admin.ViewModels;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")] // 管理者
    public class DashboardController : Controller
    {
        private readonly IAdminDashboardService _dashboardService;

        public DashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        //主頁

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _dashboardService.GetDashboardAsync();
            return View(vm);
        }

        //區間圖表資料 (AJAX)

        [HttpGet]
        public async Task<IActionResult> GetChartData(string range)
        {
            if (string.IsNullOrWhiteSpace(range))
                return BadRequest(new { success = false, message = "Range is required" });

            var data = await _dashboardService.GetChartDataAsync(range);

            return Json(new
            {
                success = true,
                data
            });
        }

        //歷史月份圖表 (AJAX)

        [HttpGet]
        public async Task<IActionResult> GetHistoryMonthData(string month)
        {
            if (string.IsNullOrWhiteSpace(month))
                return BadRequest(new { success = false, message = "Month is required" });

            var data = await _dashboardService.GetHistoryMonthDataAsync(month);

            return Json(new
            {
                success = true,
                data
            });
        }
    }
}