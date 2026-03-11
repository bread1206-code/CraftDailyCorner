using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly CraftDailyCornerContext _context;

        public SiteSettingService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _context.PlatformSettings
                .Where(x => x.SettingKey == key)
                .Select(x => x.SettingValue)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetIntAsync(string key)
        {
            var value = await GetStringAsync(key);

            if (int.TryParse(value, out var result))
                return result;

            return 0;
        }

        public async Task<bool> GetBoolAsync(string key)
        {
            var value = await GetStringAsync(key);

            if (bool.TryParse(value, out var result))
                return result;

            return false;
        }
    }
}