using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Homepage;

namespace CraftDailyCorner.ViewComponents
{
    public class VCHomepageBanner : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;
        private readonly ISiteSettingService _siteSetting;

        public VCHomepageBanner(
            CraftDailyCornerContext context,
            ISiteSettingService siteSetting)
        {
            _context = context;
            _siteSetting = siteSetting;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var autoplaySeconds = await _siteSetting.GetIntAsync("BannerAutoplaySeconds");

            if (autoplaySeconds <= 0)
                autoplaySeconds = 5;

            var items = await _context.HomepageBanners
                .Where(p => p.StatusID == 1)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMHomepageBannerItem
                {
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            var vm = new VMHomepageBanner
            {
                AutoplaySeconds = autoplaySeconds,
                Items = items
            };

            return View(vm);
        }
    }
}