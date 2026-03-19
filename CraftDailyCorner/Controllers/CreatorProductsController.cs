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

        // 商品列表
        public IActionResult Index()
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var products = _productService.GetCreatorProductList(creatorId);

            return View(products);
        }

        // 建立
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
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                _productService.LoadOptions(vm);
                return View(vm);
            }

            var productId = await _productService.CreateAsync(vm, creatorId);

            if (vm.ImageFiles != null && vm.ImageFiles.Any())
            {
                await _imageService.UploadAsync(
                    productId,
                    creatorId,
                    vm.ImageFiles);
            }

            TempData["ProductsSuccess"] = "商品已建立";
            return RedirectToAction(nameof(Index));
        }

        // 編輯
        public async Task<IActionResult> Edit(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var vm = await _productService.GetEditFormAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorProductForm vm)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                _productService.LoadOptions(vm);
                return View(vm);
            }

            try
            {
                var success = await _productService.UpdateAsync(vm, creatorId);

                if (!success)
                    return NotFound();

                TempData["ProductsSuccess"] = "商品已更新";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _productService.LoadOptions(vm);
                return View(vm);
            }
        }

        // 快速切換上架 / 下架
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            try
            {
                await _productService.TogglePublishStatusAsync(id, creatorId);
                TempData["ProductsSuccess"] = "商品狀態已更新";
            }
            catch (Exception ex)
            {
                TempData["ProductsWarning"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // 舊的 Delete 可先保留不用，或之後刪掉
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var vm = await _productService.GetEditFormAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            vm.StatusID = 3; // OffSale

            await _productService.UpdateAsync(vm, creatorId);

            TempData["ProductsSuccess"] = "商品已下架";
            return RedirectToAction(nameof(Index));
        }
    }
}