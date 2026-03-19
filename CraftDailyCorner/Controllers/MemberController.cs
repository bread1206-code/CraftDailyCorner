using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberCenterService _memberCenterService;
        private readonly IFavoriteService _favoriteService;
        private readonly IFollowService _followService;
        private readonly IMemberSecurityService _memberSecurityService;

        public MemberController(
            IMemberCenterService memberCenterService,
            IFavoriteService favoriteService,
            IFollowService followService,
            IMemberSecurityService memberSecurityService)
        {
            _memberCenterService = memberCenterService;
            _favoriteService = favoriteService;
            _followService = followService;
            _memberSecurityService = memberSecurityService;
        }

        // GET: /Member
        public IActionResult Index()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var vm = _memberCenterService.GetDashboard(memberId);
            return View(vm);
        }

        // GET: /Member/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var vm = _memberCenterService.GetProfile(memberId);
            return View(vm);
        }

        // POST: /Member/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileAsync(VMEditProfile vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            try
            {
                _memberCenterService.UpdateProfile(memberId, vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Phone", ex.Message);
                return View(vm);
            }

            var claims = User.Claims
                .Where(c => c.Type != ClaimTypes.Name)
                .ToList();

            claims.Add(new Claim(ClaimTypes.Name, vm.DisplayName));
            var identity = new ClaimsIdentity(claims, "CraftDailyCornerLogin");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CraftDailyCornerLogin", principal);

            TempData["MemberProfileSuccess"] = "個人資料已更新";
            return RedirectToAction(nameof(Profile));
        }

        // GET: /Member/Favorites
        public IActionResult Favorites()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var favorites = _favoriteService.GetMyFavorites(memberId);
            return View(favorites);
        }

        // GET: /Member/Follows
        public async Task<IActionResult> Follows()
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var list = await _followService.GetMyFollowingAsync(memberId);
            return View(list);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new VMChangePassword());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(VMChangePassword vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var (ok, message) = await _memberSecurityService.ChangePasswordAsync(
                memberId,
                vm.CurrentPassword,
                vm.NewPassword);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(vm);
            }

            TempData["ChangePasswordSuccess"] = message;
            return RedirectToAction(nameof(ChangePassword));
        }
    }
}