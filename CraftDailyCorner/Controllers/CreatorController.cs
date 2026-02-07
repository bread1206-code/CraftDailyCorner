using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front.CreatorApplication;
using CraftDailyCorner.ViewModels.Front.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CraftDailyCorner.Extensions;

namespace CraftDailyCorner.Controllers.Front
{
    [Authorize]
    public class CreatorController : Controller
    {
        private readonly CreatorApplicationService _creatorApplicationService;
        private readonly IImageUploadService _imageUploadService;

        public CreatorController(
            CreatorApplicationService creatorApplicationService,
            IImageUploadService imageUploadService)
        {
            _creatorApplicationService = creatorApplicationService;
            _imageUploadService = imageUploadService;
        }

        // GET: /Creator/Apply
        public IActionResult Apply()
        {
            var memberId = User.GetMemberId();

            var latest = _creatorApplicationService.GetLatestByMember(memberId);

            if (latest != null)
            {
                switch (latest.CreatorApplicationStatus.StatusCode)
                {
                    case "Pending":
                        return View("Pending", new VMCreatorApplicationPending
                        {
                            DisplayName = latest.DisplayName,
                            AppliedAt = latest.AppliedAt
                        });

                    case "Approved":
                        return View("Approved", new VMCreatorApplicationApproved
                        {
                            DisplayName = latest.DisplayName,
                            ReviewedAt = latest.ReviewedAt!.Value
                        });
                }
            }

            return View(new VMCreatorApplicationApply
            {
                StartDate = DateTime.Today
            });
        }

        // POST: /Creator/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(VMCreatorApplicationApply vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var memberId = User.GetMemberId();

            // 防止重複申請（雙保險）
            if (_creatorApplicationService.HasPending(memberId))
            {
                ModelState.AddModelError(string.Empty, "你已有一筆審核中的創作者申請");
                return View(vm);
            }

            if (vm.StartDate > DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(vm.StartDate),
                    "創作起始日不可為未來日期");
                return View(vm);
            }

            string imageKey;

            try
            {
                imageKey = _imageUploadService.UploadImage(
                    file: vm.PortfolioSample,
                    seedSourcePath: null,
                    folderName: "CreatorApplication",
                    sizes: ImageSizePresets.CreatorApplication,
                    entityId: null
                );
            }
            catch (Exception ex)
            {
                // 將圖片錯誤轉為表單錯誤訊息
                ModelState.AddModelError(
                    nameof(vm.PortfolioSample),
                    ex.Message);
                return View(vm);
            }

            _creatorApplicationService.Create(new CreatorApplicationCreateDTO
            {
                MemberId = memberId,
                DisplayName = vm.DisplayName,
                Intro = vm.Intro,
                PortfolioSampleUrl = imageKey,
                StartDate = vm.StartDate
            });

            return RedirectToAction(nameof(ApplySuccess));
        }

        // GET: /Creator/ApplySuccess
        public IActionResult ApplySuccess()
        {
            return View(new VMCreatorApplicationSuccess
            {
                AppliedAt = DateTime.Now
            });
        }
    }
}