using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers.Front
{
    public class PortfolioController : Controller
    {
        private readonly ICreatorPortfolioService _portfolioService;

        public PortfolioController(ICreatorPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        // 前台列表
        public async Task<IActionResult> Index(string? PortfolioKeyword, int page = 1)
        {
            var query = new VMPortfolioIndexQuery
            {
                PortfolioKeyword = PortfolioKeyword,
                Page = page,
                PageSize = 9
            };

            var currentMemberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var vm = await _portfolioService.GetPortfolioIndexAsync(query, currentMemberId);

            return View(vm);
        }

        // 前台單篇
        public async Task<IActionResult> Detail(string id)
        {
            var currentMemberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var canView = await _portfolioService.CanViewPortfolioAsync(id, currentMemberId);

            if (!canView)
                return Forbid();

            var vm = await _portfolioService.GetPublicPortfolioDetailAsync(id, currentMemberId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        // 後台列表
        [Authorize(Roles = "02")]
        public async Task<IActionResult> List()
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var portfolios = await _portfolioService.GetCreatorPortfoliosAsync(creatorId);

            return View(portfolios);
        }

        // 建立
        [Authorize(Roles = "02")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMCreatorPortfolioCreate vm)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return View(vm);

            await _portfolioService.CreateAsync(
                new CreateCreatorPortfolioDTO
                {
                    Title = vm.Title,
                    Description = vm.Description ?? "",
                    Visibility = vm.Visibility
                },
                creatorId,
                vm.Files ?? new List<IFormFile>()
            );

            return RedirectToAction(nameof(List));
        }

        // 編輯
        [Authorize(Roles = "02")]
        public async Task<IActionResult> Edit(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var vm = await _portfolioService.GetEditDataAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorPortfolioEdit vm)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            if (!ModelState.IsValid)
                return View(vm);

            await _portfolioService.UpdateAsync(
                new UpdateCreatorPortfolioDTO
                {
                    PortfolioID = vm.PortfolioID,
                    Title = vm.Title,
                    Description = vm.Description ?? "",
                    Visibility = vm.Visibility
                },
                creatorId
            );

            return RedirectToAction(nameof(List));
        }

        // 軟刪除
        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            await _portfolioService.SoftDeleteAsync(id, creatorId);

            return RedirectToAction(nameof(List));
        }
    }
}