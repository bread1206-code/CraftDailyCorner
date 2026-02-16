using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.CreatorProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize(Roles = "02")]
    public class CreatorProductsController : Controller
    {
        private readonly CreatorProductService _productService;
        private readonly CreatorProductImageService _imageService;

        public CreatorProductsController(
            CreatorProductService productService,
            CreatorProductImageService imageService)
        {
            _productService = productService;
            _imageService = imageService;
        }

        // ==========================================
        // 商品列表
        // ==========================================
        public IActionResult Index()
        {
            var creatorId = User.GetCreatorId();

            var products = _productService
                .GetCreatorProductList(creatorId);

            return View(products);
        }

        // ==========================================
        // 建立
        // ==========================================
        public IActionResult Create()
        {
            var vm = _productService.GetCreateForm();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMCreatorProductForm vm)
        {
            var creatorId = User.GetCreatorId();

            if (!ModelState.IsValid)
            {
                // 只重建選單，不覆蓋使用者輸入
                _productService.LoadOptions(vm);
                return View(vm);
            }

            // 建立商品
            var productId = await _productService
                .CreateAsync(vm, creatorId);

            // 多圖上傳
            if (vm.ImageFiles != null && vm.ImageFiles.Any())
            {
                await _imageService.UploadAsync(
                    productId,
                    creatorId,
                    vm.ImageFiles);
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 編輯
        // ==========================================
        public async Task<IActionResult> Edit(string id)
        {
            var creatorId = User.GetCreatorId();

            var vm = await _productService
                .GetEditFormAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorProductForm vm)
        {
            var creatorId = User.GetCreatorId();

            if (!ModelState.IsValid)
            {
                _productService.LoadOptions(vm);
                return View(vm);
            }

            var success = await _productService
                .UpdateAsync(vm, creatorId);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 軟刪除（下架）
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var creatorId = User.GetCreatorId();

            var vm = await _productService
                .GetEditFormAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            vm.StatusID = 3; // OffSale

            await _productService
                .UpdateAsync(vm, creatorId);

            return RedirectToAction(nameof(Index));
        }
    }
}