using CraftDailyCorner.Areas.Admin.ViewModels.Violation;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminViolationService
    {
        // mode:
        // - pending：待審核（StatusID=1）
        // - history：歷史資料（StatusID!=1） + MemberID 搜尋（檢舉人/被檢舉者）
        Task<VMAdminViolationIndex> GetIndexAsync(string mode, string? memberId = null, int page = 1);

        Task<VMAdminViolationDetail?> GetDetailAsync(long reportId);

        // 1) 判斷事件違規 -> Report.StatusID=2 -> 更新目標狀態
        Task MarkViolationAsync(long reportId, string adminMemberId, string? adminNote);

        // 2) 判斷事件沒有違規 -> Report.StatusID=3
        Task MarkNormalAsync(long reportId, string adminMemberId, string? adminNote, bool isMalicious);

        // 下一筆待審核（提高效率）
        Task<long?> GetNextPendingIdAsync(long currentReportId, string v);
    }
}