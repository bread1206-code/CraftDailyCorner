using CraftDailyCorner.ViewModels.Creator;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPublicService
    {
        Task<VMCreatorPublicProfile?> GetProfileAsync(string creatorId, string? memberId, string? loginCreatorId);

        Task<VMCreatorIndex> GetCreatorIndexAsync(string? CreatorKeyword, int page);
    }
}
