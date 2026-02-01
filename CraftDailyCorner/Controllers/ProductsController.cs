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
