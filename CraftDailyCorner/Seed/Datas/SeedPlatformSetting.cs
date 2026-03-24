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
        public void Run()
        {
            if (!_context.PlatformSettings.Any()) // 避免重複 Seed
            {
                var platformSettings = new List<PlatformSetting>
                {
                    new PlatformSetting
                    {
                        SettingKey = "PlatformName",
                        SettingValue = "CraftDailyCorner",
                        DataType = "string",
                        CategoryID = 1,
                        Description = "平台顯示名稱",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "PlatformServiceEmail",
                        SettingValue = "service@craftdailycorner.com",
                        DataType = "string",
                        CategoryID = 1,
                        Description = "客服聯絡信箱",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "HomepageFeaturedProductCount",
                        SettingValue = "8",
                        DataType = "int",
                        CategoryID = 2,
                        Description = "發燒新品展示數量",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "BannerAutoplaySeconds",
                        SettingValue = "5",
                        DataType = "int",
                        CategoryID = 2,
                        Description = "Banner 輪播秒數",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "ProductListPageSize",
                        SettingValue = "12",
                        DataType = "int",
                        CategoryID = 2,
                        Description = "商品列表每頁筆數",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "RegistrationEnabled",
                        SettingValue = "true",
                        DataType = "bool",
                        CategoryID = 1,
                        Description = "是否開放會員註冊",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    },
                    new PlatformSetting
                    {
                        SettingKey = "PlatformLogo",
                        SettingValue = "platformLogo",
                        DataType = "image",
                        CategoryID = 2,
                        Description = "平台 LOGO 圖片",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000000"
                    }
                };
                _context.PlatformSettings.AddRange(platformSettings);
                _context.SaveChanges();
            }
        }
    }
}
