using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;

namespace test.ViewComponents
{
    public class VCHomepageBanner : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;

        public VCHomepageBanner(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var HomepageBanner = await _context.HomepageBanners
            .Where(p => p.Status == (HomepageBannerStatus)1)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new HomepageBanner
            {
                Title = p.Title,
                Subtitle = p.Subtitle,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();
            return View(HomepageBanner);
        }
    }

}
