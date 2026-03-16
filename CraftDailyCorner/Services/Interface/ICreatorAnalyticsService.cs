using CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Community;
using CraftDailyCorner.ViewModels.CreatorAnalytics.Common;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorAnalyticsService
    {
        Task<VMCommunityDashboard> GetCommunityDashboardAsync(string creatorId);
        Task<VMCommerceDashboard> GetCommerceDashboardAsync(string creatorId);

        // Commerce - AJAX 圖表
        Task<VMAnalyticsChartResponse> GetCommerceRevenueTrendAsync(string creatorId, VMAnalyticsChartQuery query);
        Task<VMAnalyticsChartResponse> GetCommerceOrderTrendAsync(string creatorId, VMAnalyticsChartQuery query);

        // Community - AJAX 圖表
        Task<VMAnalyticsChartResponse> GetCommunityPostTrendAsync(string creatorId, VMAnalyticsChartQuery query);
        Task<VMAnalyticsChartResponse> GetCommunityPortfolioTrendAsync(string creatorId, VMAnalyticsChartQuery query);
        Task<VMAnalyticsChartResponse> GetCommunityCommentTrendAsync(string creatorId, VMAnalyticsChartQuery query);
        Task<VMAnalyticsChartResponse> GetCommunityReactionTrendAsync(string creatorId, VMAnalyticsChartQuery query);
    }
}