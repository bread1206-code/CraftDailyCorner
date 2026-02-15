using CraftDailyCorner.DTOs;
using CraftDailyCorner.ViewModels.CreatorPost;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPostService
    {
        //取得創作者所有日誌（後台列表）
        Task<List<VMCreatorPostListItem>> GetCreatorPostsAsync(string creatorId);

        //取得單筆日誌（編輯頁用）
        Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId);

        //建立日誌
        Task CreateAsync(CreateCreatorPostDTO dto, string creatorId);

        //更新日誌
        Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId);

        // 軟刪除日誌
        Task SoftDeleteAsync(string postId, string creatorId);

        Task<VMPostIndex> GetPostIndexAsync(VMPostIndexQuery query);
        Task<VMPostDetail?> GetPublicPostDetailAsync(string postId);
        // 判斷是否可以觀看日誌
        Task<bool> CanViewPostAsync(string postId, string? memberId);
        Task<VMPostDetail?> GetPostDetailAsync(string postId); 
    }
}
