using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Creator;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers.Front
{
    [Authorize]
    public class CreatorController : Controller
    {
        private readonly ICreatorApplicationService _applicationService;
        private readonly ICreatorDashboardService _dashboardService;
        private readonly IImageUploadService _imageUploadService;
        private readonly ICreatorPublicService _creatorPublicService;
        private readonly ICreatorProfileService _creatorProfileService;
        private readonly IAuthService _authService;

        public CreatorController(
            ICreatorApplicationService applicationService,
            ICreatorDashboardService dashboardService,
            IImageUploadService imageUploadService,
            ICreatorPublicService creatorPublicService,
            ICreatorProfileService creatorProfileService,
            IAuthService authService)
        {
            _applicationService = applicationService;
            _dashboardService = dashboardService;
            _imageUploadService = imageUploadService;
            _creatorPublicService = creatorPublicService;
            _creatorProfileService = creatorProfileService;
            _authService = authService;
        }

        // =============================
        // 創作者申請
        // =============================

        public async Task<IActionResult> Apply()
        {
            var result = await _applicationService
                .GetApplyPageAsync(User.GetMemberId());

            return result switch
            {
                VMCreatorApplicationPending vm => View("Pending", vm),
                VMCreatorApplicationApproved vm => View("Approved", vm),
                _ => View(result)
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(VMCreatorApplicationApply vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string imageKey;
            try
            {
                imageKey = _imageUploadService.UploadImage(
                    vm.PortfolioSample,
                    null,
                    "02CreatorApplication",
                    ImageSizePresets.CreatorApplication
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(vm.PortfolioSample), ex.Message);
                return View(vm);
            }

            await _applicationService.CreateAsync(new CreatorApplicationCreateDTO
            {
                MemberId = User.GetMemberId(),
                BrandName = vm.BrandName,
                BrandIntro = vm.BrandIntro,
                PortfolioSampleUrl = imageKey,
                StartDate = vm.StartDate
            });

            return RedirectToAction("Index", "Member");
        }

        // =============================
        // 創作者中心
        // =============================

        [Authorize(Roles = "02")]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await _dashboardService
                .GetDashboardAsync(User.GetMemberId());

            if (vm == null)
                return RedirectToAction("Index", "Member");

            return View(vm);
        }

        // =============================
        // 創作者公開頁/列表
        // =============================

        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            var memberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var loginCreatorId = User.Identity?.IsAuthenticated == true
                ? User.GetCreatorId()
                : null;

            var creator = await _creatorPublicService
                .GetProfileAsync(id, memberId, loginCreatorId);

            if (creator == null)
                return NotFound();

            return View(creator);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? keyword, int page = 1)
        {
            var vm = await _creatorPublicService.GetCreatorIndexAsync(keyword, page);
            return View(vm);
        }

        // =============================
        // 創作者品牌資料編輯
        // =============================

        [Authorize(Roles = "02")]
        [HttpGet]
        public async Task<IActionResult> BrandEdit()
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrEmpty(creatorId)) return Unauthorized();

            var vm = await _creatorProfileService.GetBrandEditAsync(creatorId);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [Authorize(Roles = "02")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BrandEdit(VMCreatorBrandEdit vm)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrEmpty(creatorId)) return Unauthorized();

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _creatorProfileService.UpdateBrandAsync(creatorId, vm);
                TempData["Success"] = "品牌資料已更新";
                return RedirectToAction(nameof(BrandEdit));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        // =============================
        // Approved Confirm（通過後確認）
        // =============================

        [HttpGet]
        public async Task<IActionResult> ApprovedConfirm(int? applicationId = null)
        {
            var memberId = User.GetMemberId();

            var vm = await _applicationService.GetApprovedConfirmAsync(memberId, applicationId);
            if (vm == null)
            {
                TempData["Warning"] = "目前沒有可確認的『已通過』申請。";
                return RedirectToAction("Index", "Member");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovedConfirm(VMApprovedConfirm vm)
        {
            var memberId = User.GetMemberId();

            if (vm.BrandImageFile == null || vm.BrandImageFile.Length == 0)
                ModelState.AddModelError("BrandImageFile", "請上傳品牌圖片");

            if (!ModelState.IsValid)
            {
                var readonlyVm = await _applicationService.GetApprovedConfirmAsync(memberId, vm.ApplicationID);
                if (readonlyVm != null)
                {
                    vm.BrandName = readonlyVm.BrandName;
                    vm.BrandIntro = readonlyVm.BrandIntro;
                    vm.StartDate = readonlyVm.StartDate;
                }
                return View(vm);
            }

            try
            {
                await _applicationService.SubmitApprovedConfirmAsync(memberId, vm);

                // ✅ 核心：刷新 Cookie claims（Role=02 / CreatorID）=> Navbar 立刻顯示創作者入口
                await _authService.RefreshSignInAsync(HttpContext, memberId);

                TempData["Success"] = "創作者資料建立完成！歡迎加入創作者行列。";
                return RedirectToAction("Dashboard", "Creator");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var readonlyVm = await _applicationService.GetApprovedConfirmAsync(memberId, vm.ApplicationID);
                if (readonlyVm != null)
                {
                    vm.BrandName = readonlyVm.BrandName;
                    vm.BrandIntro = readonlyVm.BrandIntro;
                    vm.StartDate = readonlyVm.StartDate;
                }
                return View(vm);
            }
        }

        // =============================
        // Rejected Confirm（拒絕後確認）
        // =============================

        [HttpGet]
        public async Task<IActionResult> RejectedConfirm(int? applicationId = null)
        {
            var memberId = User.GetMemberId();

            var vm = await _applicationService.GetRejectedConfirmAsync(memberId, applicationId);
            if (vm == null)
            {
                TempData["Warning"] = "目前沒有可確認的『已拒絕』申請。";
                return RedirectToAction("Index", "Member");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectedConfirm(VMRejectedConfirm vm)
        {
            var memberId = User.GetMemberId();

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _applicationService.SubmitRejectedConfirmAsync(memberId, vm.ApplicationID);
                TempData["Success"] = "已確認申請結果。期待你調整後再次申請！";
                return RedirectToAction("Index", "Member");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var readonlyVm = await _applicationService.GetRejectedConfirmAsync(memberId, vm.ApplicationID);
                if (readonlyVm != null)
                {
                    vm.BrandName = readonlyVm.BrandName;
                    vm.ReviewNote = readonlyVm.ReviewNote;
                }
                return View(vm);
            }
        }
    }
}