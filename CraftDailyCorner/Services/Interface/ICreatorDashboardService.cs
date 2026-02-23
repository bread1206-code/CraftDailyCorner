using CraftDailyCorner.ViewModels.Creator;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorDashboardService
    {
        // 取得創作者後台 Dashboard 資料
        Task<VMCreatorDashboard?> GetDashboardAsync(string memberId);

    }
}
