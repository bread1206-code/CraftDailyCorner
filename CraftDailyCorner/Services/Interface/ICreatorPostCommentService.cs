using CraftDailyCorner.DTOs;
using CraftDailyCorner.Services.ReportCommentRe;
using CraftDailyCorner.ViewModels.CreatorPost;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPostCommentService
    {
        Task<VMPostCommentItem> CreateAsync(CreatePostCommentDTO dto,string memberId,string? creatorId = null);

        Task<List<VMPostCommentItem>> GetPostCommentsAsync( string postId,string? currentMemberId,string? currentCreatorId);

        Task<ReportCommentResponse> ReportAsync(ReportPostCommentDTO dto,string reporterId);

        Task<VMPostCommentItem> BuildCommentViewModelAsync(string commentId,string? currentMemberId,string? currentCreatorId);
    }
}