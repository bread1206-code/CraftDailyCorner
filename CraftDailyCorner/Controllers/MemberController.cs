using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
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

        public MemberController(
            IMemberCenterService memberCenterService,
            IFavoriteService favoriteService,
            IFollowService followService)
        {
            _memberCenterService = memberCenterService;
            _favoriteService = favoriteService;
            _followService = followService;
        }

        // GET: /Member
        public IActionResult Index()
        {
            var memberId = User.GetMemberId();
            var vm = _memberCenterService.GetDashboard(memberId);
            return View(vm);
        }

        // GET: /Member/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            var memberId = User.GetMemberId();
            var vm = _memberCenterService.GetProfile(memberId);
            return View(vm);
        }

        // POST: /Member/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileAsync(CraftDailyCorner.ViewModels.Member.VMEditProfile vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = User.GetMemberId();
            //手機重複檢查
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
            .Where(c => c.Type != ClaimTypes.Name) // 移除舊的 Name
            .ToList();

            claims.Add(new Claim(ClaimTypes.Name, vm.DisplayName));
            var identity = new ClaimsIdentity(claims, "CraftDailyCornerLogin");
            var principal = new ClaimsPrincipal(identity);

            // 重新簽入（刷新 Cookie）
            await HttpContext.SignInAsync("CraftDailyCornerLogin", principal);

            TempData["Success"] = "個人資料已更新";
            return RedirectToAction(nameof(Profile));
        }

        // GET: /Member/Favorites
        public IActionResult Favorites()
        {
            var memberId = User.GetMemberId();
            var favorites = _favoriteService.GetMyFavorites(memberId);
            return View(favorites);
        }

        // GET: /Member/Follows
        public async Task<IActionResult> Follows()
        {
            var memberId = User.GetMemberId();
            var list = await _followService.GetMyFollowingAsync(memberId);
            return View(list);
        }
    }
}