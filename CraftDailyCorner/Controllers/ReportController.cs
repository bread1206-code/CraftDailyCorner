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

            switch (result.Result)
            {
                case ReportResponseEnum.Success:
                    TempData["Success"] = "檢舉已送出";
                    break;

                case ReportResponseEnum.AlreadyReported:
                    TempData["Warning"] = "您已檢舉過此內容";
                    break;

                case ReportResponseEnum.Forbidden:
                    return Forbid();

                case ReportResponseEnum.NotFound:
                    return NotFound();
            }

            //根據檢舉類型決定導頁
            return RedirectByReportType(dto, result);
        }

        // --------------------------------------------------
        // 根據不同檢舉類型導回對應頁面
        // --------------------------------------------------
        private IActionResult RedirectByReportType(
            ReportDTO dto,
            ReportResponse result)
        {
            switch (dto.ReportType)
            {
                case ReportTargetType.Comment:
                    return RedirectToAction(
                        "Detail",
                        "Post",
                        new { id = result.TargetID });

                case ReportTargetType.Post:
                    return RedirectToAction(
                        "Detail",
                        "Post",
                        new { id = dto.TargetID });

                case ReportTargetType.Product:
                    return RedirectToAction(
                        "Detail",
                        "Product",
                        new { id = dto.TargetID });

                case ReportTargetType.Review:
                    // 評價通常在商品頁
                    return RedirectToAction(
                        "Detail",
                        "Product",
                        new { id = result.TargetID });

                case ReportTargetType.Portfolio:
                    return RedirectToAction(
                        "Detail",
                        "CreatorPortfolio",
                        new { id = dto.TargetID });

                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}