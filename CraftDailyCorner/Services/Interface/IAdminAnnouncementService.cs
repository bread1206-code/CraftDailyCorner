using CraftDailyCorner.Areas.Admin.ViewModels.Announcement;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminAnnouncementService
    {
        Task<VMAdminAnnouncementIndex> GetIndexAsync();

        Task<VMAdminAnnouncementUpsert> GetCreateVmAsync(string currentMemberId, bool isSuperAdmin);

        Task<VMAdminAnnouncementUpsert?> GetEditVmAsync(int id, string currentMemberId, bool isSuperAdmin);

        Task<VMAdminAnnouncementDetail?> GetDetailAsync(int id);

        Task<int> CreateAsync(VMAdminAnnouncementUpsert vm, string currentMemberId, bool isSuperAdmin);

        Task<bool> UpdateAsync(VMAdminAnnouncementUpsert vm, string currentMemberId, bool isSuperAdmin);

        Task<(bool ok, string? message)> ActivateAsync(int id, string currentMemberId, bool isSuperAdmin);

        Task<(bool ok, string? message)> InactivateAsync(int id, string currentMemberId, bool isSuperAdmin);
    }
}