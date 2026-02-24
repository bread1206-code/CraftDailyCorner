using CraftDailyCorner.ViewModels.CreatorAnalytics;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorAnalyticsService
    {
        Task<VMCommunityDashboard> GetCommunityDashboardAsync(string creatorId);
    }
}
