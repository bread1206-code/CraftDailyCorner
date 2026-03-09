using CraftDailyCorner.Areas.Admin.ViewModels.HomepageBanner;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminHomepageBannerService
    {
        Task<VMAdminHomepageBannerIndex> GetIndexAsync();

        Task<VMAdminHomepageBannerUpsert> GetCreateVmAsync();

        Task<VMAdminHomepageBannerUpsert?> GetEditVmAsync(int id);

        Task CreateAsync(VMAdminHomepageBannerUpsert vm, string adminMemberId);

        Task<bool> UpdateAsync(VMAdminHomepageBannerUpsert vm);

        Task<(bool ok, string? message)> DisableAsync(int id);

        Task<(bool ok, string? message)> EnableAsync(int id);
    }
}