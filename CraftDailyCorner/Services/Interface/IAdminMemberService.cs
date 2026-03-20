using CraftDailyCorner.Areas.Admin.ViewModels.Member;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAdminMemberService
    {
        Task<VMAdminMemberIndex> GetIndexAsync(string mode = "all", string? memberId = null, int page = 1);
        Task<VMAdminMemberDetail?> GetDetailAsync(string memberId, string mode = "all");

        Task<VMAdminAssignGeneralAdmin> GetAssignGeneralAdminAsync(string? phone, string operatorMemberId);
        Task<(bool ok, string? message)> AssignGeneralAdminAsync(string memberId, string operatorMemberId);

        Task<(bool ok, string? message)> SuspendAsync(string memberId, string adminMemberId);
        Task<(bool ok, string? message)> ActivateAsync(string memberId, string adminMemberId);
    }
}