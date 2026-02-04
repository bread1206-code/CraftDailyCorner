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
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.CreatorProfile)
            .Include(p => p.Inventory) 
            .FirstOrDefault(p => p.ProductID == id && p.StatusID == 2);


            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new VMProductDetail
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,

                StockQty = product.Inventory?.StockQty ?? 0,
                AlertQty = product.Inventory?.AlertQty ?? 0,

                ImageUrls = product.ProductImages
                    .Where(i => i.StatusID == 1)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .ToList(),

                CreatorId = product.CreatorProfile?.CreatorID,
                CreatorName = product.CreatorProfile?.DisplayName,

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
