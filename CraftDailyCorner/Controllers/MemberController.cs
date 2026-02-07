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
        private readonly IImageUploadService _imageUploadService;

        public MemberController(MemberCenterService memberCenterService, IImageUploadService imageUploadService)
        {
            _memberCenterService = memberCenterService;
            _imageUploadService = imageUploadService;
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
        // 個人資料送出
        // POST: /Member/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(VMEditProfile vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = GetMemberId();

            // 有上傳頭像才處理
            if (vm.AvatarFile != null && vm.AvatarFile.Length > 0)
            {
                var fileName = _imageUploadService.UploadImage(
                    file: vm.AvatarFile,
                    seedSourcePath: null,
                    folderName: "01Member",
                    sizes: ImageSizePresets.Member,
                    entityId: memberId // 會員頭像用 MemberID 當檔名
                );

                //回寫到 ViewModel
                vm.ImageUrl = fileName;
            }

            //更新會員資料
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