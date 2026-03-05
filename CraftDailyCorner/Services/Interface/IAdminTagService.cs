using CraftDailyCorner.Areas.Admin.ViewModels.Tag;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminTagService
    {
        Task<VMAdminTagIndex> GetIndexAsync();

        Task CreateAsync(VMAdminTagEdit vm);

        Task<VMAdminTagEdit?> GetEditAsync(int id);

        Task<bool> UpdateAsync(VMAdminTagEdit vm);

        Task<bool> ToggleActiveAsync(int id);
    }
}