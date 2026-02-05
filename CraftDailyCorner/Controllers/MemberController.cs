using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly MemberCenterService _memberCenterService;

        public MemberController(MemberCenterService memberCenterService)
        {
            _memberCenterService = memberCenterService;
        }

        //會員中心首頁
        // GET: /Member
        public IActionResult Index()
        {
            var memberId = GetMemberId();

            var vm = _memberCenterService.GetMemberDashboard(memberId);

            return View(vm);
        }

        // 個人資料
        // GET: /Member/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            var memberId = GetMemberId();
            var vm = _memberCenterService.GetProfile(memberId);
            return View(vm);
        }

        // 個人資料送出
        // POST: /Member/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(VMEditProfile vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            if(vm.AvatarFile != null)
            {
                //呼叫上傳圖片
            }
            
            _memberCenterService.UpdateProfile(vm);
            TempData["Success"] = "個人資料已更新";
            return RedirectToAction(nameof(Profile));
        }

        //私有方法：取得登入會員 ID
        private string GetMemberId()
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(memberId))
                throw new UnauthorizedAccessException("找不到會員識別資訊");

            return memberId;
        }
    }
}