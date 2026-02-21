using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize] // 只要登入會員即可
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportAsync(ReportDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("資料錯誤");

            var memberId = User.GetMemberId();

            var result = await _reportService
                .ReportAsync(dto, memberId);

            return Json(new
            {
                result = result.Result.ToString()
            });
        }

    }
}