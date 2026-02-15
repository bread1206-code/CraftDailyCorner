using CraftDailyCorner.DTOs;
using CraftDailyCorner.ViewModels.CreatorPost;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPostService
    {
        // 前台
        Task<VMPostIndex> GetPostIndexAsync(VMPostIndexQuery query);
        Task<VMPostDetail?> GetPostDetailAsync(string postId);
        Task<bool> CanViewPostAsync(string postId, string? memberId);

        // 後台
        Task<List<VMPostListItem>> GetCreatorPostsAsync(string creatorId);

        Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId);

        // 建立
        Task CreateAsync(CreateCreatorPostDTO dto, string creatorId);

        // 更新
        Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId);

        // 軟刪除
        Task SoftDeleteAsync(string postId, string creatorId);
    }
}