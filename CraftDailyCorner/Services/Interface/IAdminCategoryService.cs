using CraftDailyCorner.Areas.Admin.ViewModels.Category;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminCategoryService
    {
        Task<VMAdminCategoryIndex> GetIndexAsync();

        Task<VMAdminCategoryUpsert> GetCreateVmAsync();
        Task CreateAsync(VMAdminCategoryUpsert vm);

        Task<VMAdminCategoryUpsert?> GetEditVmAsync(int id);
        Task<bool> UpdateAsync(VMAdminCategoryUpsert vm);

        //軟刪除（停用）
        Task<(bool ok, string? message)> DisableAsync(int id);

        //從軟刪除復原（啟用）
        Task<(bool ok, string? message)> EnableAsync(int id);
    }
}