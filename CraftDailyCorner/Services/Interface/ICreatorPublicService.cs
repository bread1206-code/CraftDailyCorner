using CraftDailyCorner.ViewModels.Creator;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPublicService
    {
        Task<VMCreatorPublicProfile?> GetProfileAsync(
            string creatorId,
            string? memberId);
        Task<VMCreatorIndex> GetCreatorIndexAsync();
    }
}
