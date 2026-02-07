using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;

namespace CraftDailyCorner.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly CraftDailyCornerContext _context;


        public SiteSettingService(CraftDailyCornerContext context)
        {
            _context = context;
        }

            public string GetNavbarLogo()
        {

            var logo = _context.PlatformSettings
                .Where(x => x.CategoryID == 1 && x.SettingKey == "platform_LogoURL")
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => x.SettingValue)
                .FirstOrDefault();

            return logo ?? "/images/default-logo";
        }
    }

}
