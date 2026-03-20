using CraftDailyCorner.Areas.Admin.ViewModels.Member;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class MembersController : Controller
    {
        private readonly IAdminMemberService _service;

        public MembersController(IAdminMemberService service)
        {
            _service = service;
        }

        // mode:
        // - risk：會員管理（ViolationCount > 5）
        // - all：查看會員資料（全部會員）
        // - admin：管理管理者（僅 Role 04）
        // - creator：查看創作者資料
        public async Task<IActionResult> Index(string mode = "all", string? memberId = null, int page = 1)
        {
            mode = (mode ?? "all").Trim().ToLower();

            if (mode == "admin" && !User.IsInRole("04"))
                return Forbid();

            var vm = await _service.GetIndexAsync(mode, memberId, page);
            return View(vm);
        }

        public async Task<IActionResult> Detail(string id, string mode = "all")
        {
            mode = (mode ?? "all").Trim().ToLower();

            if (mode == "admin" && !User.IsInRole("04"))
                return Forbid();

            var vm = await _service.GetDetailAsync(id, mode);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AssignGeneralAdmin(string? searchPhone = null)
        {
            if (!User.IsInRole("04"))
                return Forbid();

            var operatorMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(operatorMemberId))
                return Unauthorized();

            var vm = await _service.GetAssignGeneralAdminAsync(searchPhone, operatorMemberId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignGeneralAdmin(string memberId, string? searchPhone = null)
        {
            if (!User.IsInRole("04"))
                return Forbid();

            var operatorMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(operatorMemberId))
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                TempData["MembersWarning"] = "請先查詢會員後再進行指派。";
                return RedirectToAction(nameof(AssignGeneralAdmin), new { phone = searchPhone });
            }

            var (ok, message) = await _service.AssignGeneralAdminAsync(memberId, operatorMemberId);

            if (!ok)
            {
                TempData["MembersWarning"] = message ?? "指派一般管理者失敗";
                return RedirectToAction(nameof(AssignGeneralAdmin), new { phone = searchPhone });
            }

            TempData["MembersSuccess"] = "已成功賦予一般管理者角色";
            return RedirectToAction(nameof(Index), new { mode = "admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suspend(string id, string mode = "all")
        {
            var adminMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(adminMemberId))
                return Unauthorized();

            var (ok, message) = await _service.SuspendAsync(id, adminMemberId);

            if (!ok)
            {
                TempData["MembersWarning"] = message ?? "停權失敗";
                return RedirectToAction(nameof(Detail), new { id, mode });
            }

            TempData["MembersSuccess"] = "會員已停權";
            return RedirectToAction(nameof(Detail), new { id, mode });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id, string mode = "all")
        {
            var adminMemberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(adminMemberId))
                return Unauthorized();

            var (ok, message) = await _service.ActivateAsync(id, adminMemberId);

            if (!ok)
            {
                TempData["MembersWarning"] = message ?? "啟用失敗";
                return RedirectToAction(nameof(Detail), new { id, mode });
            }

            TempData["MembersSuccess"] = "會員已恢復啟用";
            return RedirectToAction(nameof(Detail), new { id, mode });
        }
    }
}