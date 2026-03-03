using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
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

        public CreatorController(
            ICreatorApplicationService applicationService,
            ICreatorDashboardService dashboardService,
            IImageUploadService imageUploadService,
            ICreatorPublicService creatorPublicService,
            ICreatorProfileService creatorProfileService)
        {
            _applicationService = applicationService;
            _dashboardService = dashboardService;
            _imageUploadService = imageUploadService;
            _creatorPublicService = creatorPublicService;
            _creatorProfileService = creatorProfileService;
        }

        //創作者申請

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
                DisplayName = vm.DisplayName,
                Intro = vm.Intro,
                PortfolioSampleUrl = imageKey,
                StartDate = vm.StartDate
            });

            return RedirectToAction("Index", "Member");
        }

        //創作者中心

        [Authorize(Roles = "02")]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await _dashboardService
                .GetDashboardAsync(User.GetMemberId());

            if (vm == null)
                return RedirectToAction("Index", "Member");

            return View(vm);
        }
        //創作者公開頁
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
        //創作者列表首頁
        public async Task<IActionResult> Index(string? keyword, int page = 1)
        {
            var vm = await _creatorPublicService.GetCreatorIndexAsync(keyword, page);
            return View(vm);
        }

        // 創作者品牌資料編輯

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

            // 防止被改 DisplayName：就算前端被改也無效（service 不更新它）
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
    }
}