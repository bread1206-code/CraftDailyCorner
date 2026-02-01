using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPlatformSettingCategory
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPlatformSettingCategory(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PlatformSettingCategories.Any()) return;

            _context.PlatformSettingCategories.AddRange(
                new PlatformSettingCategory { CategoryID = 1, CategoryCode = "General", CategoryName = "一般設定" },
                new PlatformSettingCategory { CategoryID = 2, CategoryCode = "Appearance", CategoryName = "外觀設定" },
                new PlatformSettingCategory { CategoryID = 3, CategoryCode = "Payment", CategoryName = "付款設定" }
            );

            _context.SaveChanges();
        }
    }

}
