using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
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

        public CreatorController(
            ICreatorApplicationService applicationService,
            ICreatorDashboardService dashboardService,
            IImageUploadService imageUploadService,
            ICreatorPublicService creatorPublicService)
        {
            _applicationService = applicationService;
            _dashboardService = dashboardService;
            _imageUploadService = imageUploadService;
            _creatorPublicService = creatorPublicService;
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
                    "CreatorApplication",
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

            var creator = await _creatorPublicService
                .GetProfileAsync(id, memberId);

            if (creator == null)
                return NotFound();

            return View(creator);
        }
        [AllowAnonymous]
        //創作者列表首頁
        public async Task<IActionResult> Index()
        {
            var vm = await _creatorPublicService
                .GetCreatorIndexAsync();

            return View(vm);
        }
    }
}