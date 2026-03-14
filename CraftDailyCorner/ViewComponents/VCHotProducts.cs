using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Homepage;
using CraftDailyCorner.Services.Interface;

namespace CraftDailyCorner.ViewComponents
{
    public class VCHotProducts : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;
        private readonly ISiteSettingService _siteSetting;

        public VCHotProducts(CraftDailyCornerContext context, ISiteSettingService siteSetting)
        {
            _context = context;
            _siteSetting = siteSetting;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featuredCount = await _siteSetting.GetIntAsync("HomepageFeaturedProductCount");

            if (featuredCount <= 0)
                featuredCount = 4;

            var hotProducts = await _context.Products
                .Where(p => p.StatusID == 2)
                .Select(p => new VMHotProductCard
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    FavoriteCount = _context.FavoriteProducts
                        .Count(f => f.ProductID == p.ProductID),
                    CoverImage = _context.ProductImages
                        .Where(i => i.ProductID == p.ProductID && i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    BrandName = p.CreatorProfile.BrandName,
                    CreatorID = p.CreatorProfile.CreatorID
                })
                .OrderByDescending(x => x.FavoriteCount)
                .ThenBy(x => x.ProductID)
                .Take(featuredCount)
                .ToListAsync();

            return View(hotProducts);
        }
    }
}