using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.CreatorApplication;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorApplicationService
    {
        //取得申請頁應顯示的 ViewModel
        Task<object> GetApplyPageAsync(string memberId);

        //是否有審核中的申請
        Task<bool> HasPendingAsync(string memberId);

        //建立創作者申請
        Task CreateAsync(CreatorApplicationCreateDTO dto);

        //取得會員最新一筆申請（後台或管理用途）
        Task<CreatorApplication?> GetLatestByMemberAsync(string memberId);

        // ===== 審核後確認流程 =====
        Task<VMApprovedConfirm?> GetApprovedConfirmAsync(string memberId, int? applicationId = null);
        Task SubmitApprovedConfirmAsync(string memberId, VMApprovedConfirm vm);

        Task<VMRejectedConfirm?> GetRejectedConfirmAsync(string memberId, int? applicationId = null);
        Task SubmitRejectedConfirmAsync(string memberId, int applicationId);
        Task ValidateBeforeCreateAsync(string memberId, string brandName);
    }
}
