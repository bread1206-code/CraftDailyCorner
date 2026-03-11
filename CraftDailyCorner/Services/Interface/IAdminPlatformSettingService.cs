using CraftDailyCorner.Areas.Admin.ViewModels.PlatformSetting;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminPlatformSettingService
    {
        Task<VMAdminPlatformSettingIndex> GetIndexAsync();

        Task<VMAdminPlatformSettingEdit?> GetEditAsync(int id);

        Task<bool> UpdateAsync(VMAdminPlatformSettingEdit vm, string adminMemberId);
    }
}