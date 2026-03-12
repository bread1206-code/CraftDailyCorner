using CraftDailyCorner.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? keyword, int? tagId, int page = 1)
        {
            string? memberId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var vm = await _productService.GetProductListAsync(
                categoryId,
                keyword,
                tagId,
                memberId,
                page
            );

            return View(vm);
        }

        public IActionResult Detail(string id)
        {
            string? memberId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var vm = _productService.GetProductDetail(id, memberId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }
    }
}