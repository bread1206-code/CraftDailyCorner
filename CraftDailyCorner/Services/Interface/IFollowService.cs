namespace CraftDailyCorner.Services.Interface
{
    public interface IFollowService
    {
        Task ToggleAsync(string creatorId, string memberId);

        Task<bool> IsFollowingAsync(string creatorId, string memberId);

        Task<int> GetFollowerCountAsync(string creatorId);
    }
}
