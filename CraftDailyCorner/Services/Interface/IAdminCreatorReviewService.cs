using CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminCreatorReviewService
    {
        Task<VMAdminCreatorReviewIndex> GetIndexAsync();
        Task<VMAdminCreatorReviewDetail?> GetDetailAsync(int applicationId);

        Task ApproveAsync(int applicationId, string adminMemberId, string? reviewNote);
        Task RejectAsync(int applicationId, string adminMemberId, string reviewNote);
    }
}
