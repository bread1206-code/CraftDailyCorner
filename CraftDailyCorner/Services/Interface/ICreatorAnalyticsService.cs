using CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Community;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorAnalyticsService
    {
        Task<VMCommunityDashboard> GetCommunityDashboardAsync(string creatorId);
        Task<VMCommerceDashboard> GetCommerceDashboardAsync(string creatorId);
    }
}
