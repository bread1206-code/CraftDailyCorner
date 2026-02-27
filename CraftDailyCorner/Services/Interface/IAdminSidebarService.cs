using CraftDailyCorner.Areas.Admin.ViewModels;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminSidebarService
    {
        Task<VMAdminSidebar> GetSidebarDataAsync();
    }
}
