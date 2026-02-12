using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Controllers.Front
{
    [Authorize(Roles = "02")]
    public class PortfolioItemController : Controller
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public PortfolioItemController(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        //List

        public async Task<IActionResult> List(string portfolioId)
        {
            var creatorId = User.GetCreatorId();

            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == portfolioId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 0);

            if (portfolio == null)
                return NotFound();

            ViewBag.PortfolioId = portfolioId;

            var items = await _context.PortfolioItems
                .Where(i => i.PortfolioID == portfolioId)
                .OrderBy(i => i.SortOrder)
                .Select(i => new VMCreatorPortfolioItemListItem
                {
                    ItemID = i.ItemID,
                    ImageUrl = i.ImageUrl,
                    SortOrder = i.SortOrder,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .ToListAsync();

            return View(items);
        }

        //Create

        public IActionResult Create(string portfolioId)
        {
            return View(new VMCreatorPortfolioItemCreate
            {
                PortfolioID = portfolioId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMCreatorPortfolioItemCreate vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var creatorId = User.GetCreatorId();

            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(p =>
                    p.PortfolioID == vm.PortfolioID &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 0);

            if (portfolio == null)
                return NotFound();

            var imageKey = _imageUploadService.UploadImage(
                vm.ImageFile,
                null,
                "Portfolio",
                ImageSizePresets.Portfolio
            );

            var entity = new PortfolioItem
            {
                ItemID = Guid.NewGuid().ToString(),
                PortfolioID = vm.PortfolioID,
                ImageUrl = imageKey,
                SortOrder = vm.SortOrder,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.PortfolioItems.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List),
                new { portfolioId = vm.PortfolioID });
        }

        //Edit

        public async Task<IActionResult> Edit(string id)
        {
            var creatorId = User.GetCreatorId();

            var item = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .FirstOrDefaultAsync(i =>
                    i.ItemID == id &&
                    i.Portfolio.CreatorID == creatorId);

            if (item == null)
                return NotFound();

            return View(new VMCreatorPortfolioItemEdit
            {
                ItemID = item.ItemID,
                PortfolioID = item.PortfolioID,
                CurrentImageUrl = item.ImageUrl,
                SortOrder = item.SortOrder,
                UpdatedAt = item.UpdatedAt
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorPortfolioItemEdit vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var creatorId = User.GetCreatorId();

            var item = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .FirstOrDefaultAsync(i =>
                    i.ItemID == vm.ItemID &&
                    i.Portfolio.CreatorID == creatorId);

            if (item == null)
                return NotFound();

            string imageKey = item.ImageUrl;

            if (vm.NewImageFile != null)
            {
                imageKey = _imageUploadService.UploadImage(
                    vm.NewImageFile,
                    null,
                    "Portfolio",
                    ImageSizePresets.Portfolio,
                    vm.ItemID
                );
            }

            item.ImageUrl = imageKey;
            item.SortOrder = vm.SortOrder;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List),
                new { portfolioId = vm.PortfolioID });
        }

        // =====================================================
        // Delete
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var creatorId = User.GetCreatorId();

            var item = await _context.PortfolioItems
                .Include(i => i.Portfolio)
                .FirstOrDefaultAsync(i =>
                    i.ItemID == id &&
                    i.Portfolio.CreatorID == creatorId);

            if (item == null)
                return NotFound();

            var portfolioId = item.PortfolioID;

            _context.PortfolioItems.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List),
                new { portfolioId });
        }
    }
}