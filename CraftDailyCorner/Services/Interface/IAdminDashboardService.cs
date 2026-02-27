using CraftDailyCorner.Areas.Admin.ViewModels;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminDashboardService
    {
        Task<VMDashboard> GetDashboardAsync();
        Task<object> GetChartDataAsync(string range);
        Task<object> GetHistoryMonthDataAsync(string month);
    }
}
