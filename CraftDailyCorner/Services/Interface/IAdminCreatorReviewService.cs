using CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminCreatorReviewService
    {
        // mode:
        // - "pending"：只顯示待審核(StatusID=1)
        // - "history"：預設顯示已通過/已拒絕(StatusID=2/3)
        //             若 memberId 有值，顯示該會員的「所有」申請資料
        Task<VMAdminCreatorReviewIndex> GetIndexAsync(string mode, string? memberId = null);
        Task<VMAdminCreatorReviewDetail?> GetDetailAsync(int applicationId);

        Task ApproveAsync(int applicationId, string adminMemberId, string? reviewNote);
        Task RejectAsync(int applicationId, string adminMemberId, string reviewNote);

        Task<int?> GetNextPendingIdAsync(int currentApplicationId, string adminMemberId);
    }
}