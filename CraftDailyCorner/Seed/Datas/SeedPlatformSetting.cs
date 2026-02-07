using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPlatformSetting
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPlatformSetting(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.PlatformSettings.Any()) // 避免重複 Seed
            {
                var platformSettings = new List<PlatformSetting>
                {
                    new PlatformSetting
                    {
                        SettingKey = "OrderAutoCancelDays",
                        SettingValue = "7",
                        DataType = "int",
                        CategoryID = 1,
                        Description = "未付款訂單自動取消天數",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000001"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "platform_LogoURL",
                        SettingValue = imageGuids[0],
                        DataType = "string",
                        CategoryID = 1,
                        Description = "平台 Logo URL",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000001"
                    }
                };
                _context.PlatformSettings.AddRange(platformSettings);
                _context.SaveChanges();
            }
        }
    }
}
