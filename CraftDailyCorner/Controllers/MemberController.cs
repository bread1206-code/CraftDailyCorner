using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Creator;
using CraftDailyCorner.ViewModels.Member;
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
        private readonly FavoriteService _favoriteService;
        private readonly CreatorApplicationService _creatorApplicationService;

        public MemberController(MemberCenterService memberCenterService, IImageUploadService imageUploadService, 
            FavoriteService favoriteService, CreatorApplicationService creatorApplicationService)
        {
            _memberCenterService = memberCenterService;
            _imageUploadService = imageUploadService;
            _favoriteService = favoriteService;
            _creatorApplicationService = creatorApplicationService;
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

            var memberId = GetMemberId();

            if (vm.AvatarFile != null && vm.AvatarFile.Length > 0)
            {
                // 有舊圖就沿用，沒有才產生新的 GUID
                var fileKey = string.IsNullOrEmpty(vm.ImageUrl)
                    ? Guid.NewGuid().ToString()
                    : vm.ImageUrl;

                _imageUploadService.UploadImage(
                    file: vm.AvatarFile,
                    seedSourcePath: null,
                    folderName: "01Member",
                    sizes: ImageSizePresets.Member,
                    entityId: fileKey
                );

                vm.ImageUrl = fileKey;
            }

            _memberCenterService.UpdateProfile(vm);

            TempData["Success"] = "個人資料已更新";
            return RedirectToAction(nameof(Profile));
        }

        // 我的收藏頁面
        [Authorize]
        public IActionResult Favorites()
        {
            var memberId = GetMemberId();

            var favorites = _favoriteService.GetMyFavorites(memberId);

            return View(favorites);
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