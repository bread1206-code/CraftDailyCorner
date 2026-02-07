using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front.Homepage;
namespace CraftDailyCorner.ViewComponents
{
    public class VCHotProducts : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;

        public VCHotProducts(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var HPorducts = await _context.Products
            .Where(p => p.StatusID == 2)
            .Select(p => new VMHotProductCard
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                FavoriteCount = _context.FavoriteProducts
                                       .Count(f => f.ProductID == p.ProductID),
                CoverImage = _context.ProductImages
                .Where(i=>i.ProductID==p.ProductID && i.StatusID == 1)
                .OrderBy(i=>i.SortOrder)
                .Select(i => i.ImageUrl)
                .FirstOrDefault(),
                CreatorName = p.CreatorProfile.DisplayName
            })
            .Take(4)
            .OrderByDescending(x => x.FavoriteCount)
            .ToListAsync();
            return View(HPorducts);

        }
    }
}
