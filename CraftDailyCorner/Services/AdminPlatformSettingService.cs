using CraftDailyCorner.Areas.Admin.ViewModels.PlatformSetting;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminPlatformSettingService : IAdminPlatformSettingService
    {
        private readonly CraftDailyCornerContext _context;

        // 先只管理這 6 個平台參數
        private static readonly string[] AllowedKeys =
        {
            "PlatformName",
            "PlatformServiceEmail",
            "HomepageFeaturedProductCount",
            "BannerAutoplaySeconds",
            "ProductListPageSize",
            "RegistrationEnabled"
        };

        public AdminPlatformSettingService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminPlatformSettingIndex> GetIndexAsync()
        {
            var items = await _context.PlatformSettings
                .AsNoTracking()
                .Include(x => x.PlatformSettingCategory)
                .Include(x => x.Member)
                .Where(x => AllowedKeys.Contains(x.SettingKey))
                .OrderBy(x => x.CategoryID)
                .ThenBy(x => x.SettingKey)
                .Select(x => new VMAdminPlatformSettingIndexItem
                {
                    SettingID = x.SettingID,
                    SettingKey = x.SettingKey,
                    SettingValue = x.SettingValue,
                    DataType = x.DataType,
                    CategoryID = x.CategoryID,
                    CategoryName = x.PlatformSettingCategory.CategoryName,
                    Description = x.Description,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.Member.DisplayName
                })
                .ToListAsync();

            return new VMAdminPlatformSettingIndex
            {
                Items = items
            };
        }

        public async Task<VMAdminPlatformSettingEdit?> GetEditAsync(int id)
        {
            var vm = await _context.PlatformSettings
                .AsNoTracking()
                .Include(x => x.PlatformSettingCategory)
                .Include(x => x.Member)
                .Where(x => x.SettingID == id && AllowedKeys.Contains(x.SettingKey))
                .Select(x => new VMAdminPlatformSettingEdit
                {
                    SettingID = x.SettingID,
                    SettingKey = x.SettingKey,
                    SettingValue = x.SettingValue,
                    DataType = x.DataType,
                    CategoryID = x.CategoryID,
                    CategoryName = x.PlatformSettingCategory.CategoryName,
                    Description = x.Description,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.Member.DisplayName,
                    HintText = BuildHintText(x.SettingKey),
                    SuggestedRange = BuildSuggestedRange(x.SettingKey)
                })
                .FirstOrDefaultAsync();

            if (vm == null)
                return null;

            if (vm.IsBoolType)
            {
                vm.BoolOptions = BuildBoolOptions(vm.SettingValue);
            }

            return vm;
        }

        public async Task<bool> UpdateAsync(VMAdminPlatformSettingEdit vm, string adminMemberId)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            if (string.IsNullOrWhiteSpace(adminMemberId))
                throw new ArgumentException("adminMemberId 不可為空");

            var entity = await _context.PlatformSettings
                .FirstOrDefaultAsync(x => x.SettingID == vm.SettingID && AllowedKeys.Contains(x.SettingKey));

            if (entity == null)
                return false;

            var newValue = (vm.SettingValue ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(newValue))
                throw new ArgumentException("設定值不可為空");

            ValidateSettingValue(entity.DataType, newValue, entity.SettingKey);

            entity.SettingValue = NormalizeSettingValue(entity.DataType, newValue);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = adminMemberId;

            await _context.SaveChangesAsync();
            return true;
        }

        private static void ValidateSettingValue(string dataType, string value, string settingKey)
        {
            dataType = (dataType ?? string.Empty).Trim().ToLower();

            switch (dataType)
            {
                case "int":
                    if (!int.TryParse(value, out var intValue))
                        throw new ArgumentException($"「{settingKey}」必須為整數");

                    if (intValue < 0)
                        throw new ArgumentException($"「{settingKey}」不可小於 0");

                    switch (settingKey)
                    {
                        case "HomepageFeaturedProductCount":
                            if (intValue < 4 || intValue > 12)
                                throw new ArgumentException("首頁精選商品數建議介於 4 ~ 12");
                            break;

                        case "BannerAutoplaySeconds":
                            if (intValue < 3 || intValue > 10)
                                throw new ArgumentException("Banner 輪播秒數建議介於 3 ~ 10");
                            break;

                        case "ProductListPageSize":
                            if (intValue < 8 || intValue > 40)
                                throw new ArgumentException("商品列表每頁筆數建議介於 8 ~ 40");
                            break;
                    }
                    break;

                case "bool":
                    if (!bool.TryParse(value, out _))
                        throw new ArgumentException($"「{settingKey}」必須為 true 或 false");
                    break;

                case "string":
                    break;

                default:
                    throw new ArgumentException($"不支援的資料型態：{dataType}");
            }
        }

        private static string NormalizeSettingValue(string dataType, string value)
        {
            dataType = (dataType ?? string.Empty).Trim().ToLower();

            return dataType switch
            {
                "bool" => bool.Parse(value).ToString().ToLower(),
                _ => value
            };
        }
        private static SelectList BuildBoolOptions(string? selectedValue)
        {
            selectedValue = (selectedValue ?? string.Empty).Trim().ToLower();

            var items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "true", Text = "開啟" },
                    new SelectListItem { Value = "false", Text = "關閉" }
                };

            return new SelectList(items, "Value", "Text", selectedValue);
        }
        private static string? BuildHintText(string settingKey)
        {
            return settingKey switch
            {
                "PlatformName" => "平台前台顯示名稱。",
                "PlatformServiceEmail" => "顯示於平台聯絡資訊或客服用途。",
                "HomepageFeaturedProductCount" => "首頁精選商品顯示數量，建議 4 ~ 12。",
                "BannerAutoplaySeconds" => "首頁 Banner 自動輪播秒數，建議 3 ~ 10。",
                "ProductListPageSize" => "商品列表每頁顯示筆數，建議 8 ~ 40。",
                "RegistrationEnabled" => "控制是否開放新會員註冊。",
                _ => null
            };
        }

        private static string? BuildSuggestedRange(string settingKey)
        {
            return settingKey switch
            {
                "HomepageFeaturedProductCount" => "4 ~ 12",
                "BannerAutoplaySeconds" => "3 ~ 10",
                "ProductListPageSize" => "8 ~ 40",
                _ => null
            };
        }
    }
}