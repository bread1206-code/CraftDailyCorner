using CraftDailyCorner.ViewModels.Front.Creator;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorDashboardService
    {
        Task<VMCreatorDashboard?> GetDashboardAsync(string memberId);
    }
}
