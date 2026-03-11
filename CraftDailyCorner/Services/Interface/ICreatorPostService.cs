using CraftDailyCorner.DTOs;
using CraftDailyCorner.ViewModels.CreatorPost;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPostService
    {
        // 前台
        Task<VMPostIndex> GetPostIndexAsync(VMPostIndexQuery query);
        Task<VMPostDetail?> GetPostDetailAsync(string postId, string? currentMemberId);
        Task<bool> CanViewPostAsync(string postId, string? memberId);

        // 後台
        Task<List<VMPostListItem>> GetCreatorPostsAsync(string creatorId);
        Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId);

        // 建立 / 更新 / 刪除
        Task CreateAsync(CreateCreatorPostDTO dto, string creatorId);
        Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId);
        Task SoftDeleteAsync(string postId, string creatorId);
    }
}