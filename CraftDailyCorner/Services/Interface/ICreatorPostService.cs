using CraftDailyCorner.ViewModels.Front.CreatorPost;
using CraftDailyCorner.ViewModels.Front.DTOs;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPostService
    {
        Task<List<VMCreatorPostListItem>> GetCreatorPostsAsync(string creatorId);

        Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId);

        Task CreateAsync(CreateCreatorPostDTO dto, string creatorId);

        Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId);

        Task SoftDeleteAsync(string postId, string creatorId);
    }
}
