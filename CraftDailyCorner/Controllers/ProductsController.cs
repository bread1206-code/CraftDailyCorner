using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CraftDailyCornerContext _context;

        public ProductsController(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public IActionResult Index(
            int? categoryId,
            string? keyword,
            int? tagId
            )
        {
            var query = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .Where(p => p.StatusID == 2); // 上架商品

            // 1️ 分類瀏覽
            if (categoryId.HasValue)
            {
                query = query.Where(p =>
                    p.ProductCategories.Any(pc => pc.CategoryID == categoryId));
            }

            // 2️ 關鍵字搜尋
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    p.Description.Contains(keyword));
            }

            // 3 Tag 瀏覽
            if (tagId.HasValue)
            {
                query = query.Where(p =>
                    p.ProductTags.Any(pt => pt.TagID == tagId));
            }

            var vm = new VMProductList
            {
                Products = query.ToList(),
                CategoryId = categoryId,
                Keyword = keyword,
                TagId = tagId
            };

            return View(vm);
        }
        public IActionResult Detail(string id)
        {
            var product = _context.Products
                .Include(pi => pi.ProductImages)
                .Include(pc => pc.ProductCategories)
                    .ThenInclude(c =>c.Category)
                .Include(cp => cp.CreatorProfile)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new VMProductDetail
            {
                Product = product,
                Images = product.ProductImages
                .Where(img => img.StatusID == 1)//1顯示中
                .OrderBy(img => img.SortOrder)
                .ToList(),
                Creator = product.CreatorProfile!,
                Categories = product.ProductCategories
                    .Select(pc => pc.Category)
                    .ToList(),
                Tags = product.ProductTags
                    .Select(pt => pt.Tag)
                    .ToList(),
                IsFavorite = false
            };
            return View(viewModel);
        }
    }
}
