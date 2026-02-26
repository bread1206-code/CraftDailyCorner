using CraftDailyCorner.ViewModels.Member;

namespace CraftDailyCorner.Services.Interface
{
    public interface IFollowService
    {
        Task ToggleAsync(string creatorId, string memberId);

        Task<bool> IsFollowingAsync(string creatorId, string memberId);

        Task<int> GetFollowerCountAsync(string creatorId);

        Task<List<VMFollowingCreatorCard>> GetMyFollowingAsync(string memberId);
    }
}
