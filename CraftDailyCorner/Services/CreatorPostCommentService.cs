using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.EntityFrameworkCore;


namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPostCommentService : ICreatorPostCommentService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IReactionService _reactionService;

        public CreatorPostCommentService(
            CraftDailyCornerContext context, IReactionService reactionService)
        {
            _context = context;
            _reactionService = reactionService;
        }

        //建立留言
        public async Task<VMPostCommentItem> CreateAsync(
            CreatePostCommentDTO dto,
            string memberId,
            string? creatorId = null)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == dto.PostID &&
                    p.StatusID == 1);

            if (post == null)
                throw new Exception("日誌不存在");

            var content = dto.Content.Trim();

            var comment = new PostComment
            {
                CommentID = Guid.NewGuid().ToString(),
                PostID = dto.PostID,
                MemberID = memberId,
                Content = content,
                Status = PostCommentStatus.Visible,
                CreatedAt = DateTime.Now
            };

            _context.PostComments.Add(comment);
            await _context.SaveChangesAsync();

            return await BuildCommentViewModelAsync(
                comment.CommentID,
                memberId,
                creatorId);
        }

        //取得留言列表
        public async Task<List<VMPostCommentItem>> GetPostCommentsAsync(
    string postId,
    string? currentMemberId,
    string? currentCreatorId)
        {
            var comments = await _context.PostComments
                .Include(c => c.Member)
                .Where(c =>
                    c.PostID == postId &&
                    c.Status == PostCommentStatus.Visible)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new VMPostCommentItem
                {
                    CommentID = c.CommentID,
                    PostID = c.PostID,
                    MemberID = c.MemberID,
                    MemberName = c.Member.DisplayName,
                    MemberAvatar = c.Member.ImageUrl,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status,
                    IsOwner = c.MemberID == currentMemberId
                })
                .ToListAsync();

            DateTime? reportBanUntil = null;
            bool isReportBanned = false;

            if (!string.IsNullOrWhiteSpace(currentMemberId))
            {
                reportBanUntil = await _context.Members
                    .AsNoTracking()
                    .Where(m => m.MemberID == currentMemberId)
                    .Select(m => m.ReportBanUntil)
                    .FirstOrDefaultAsync();

                isReportBanned = reportBanUntil.HasValue &&
                                 reportBanUntil.Value > DateTime.Now;
            }

            // 補上每則留言的 Reaction 狀態 + 檢舉停權資訊
            foreach (var item in comments)
            {
                item.ReactionButton = await _reactionService.GetButtonStateAsync(
                    currentMemberId,
                    ReactionTargetType.PostComment,
                    item.CommentID
                );

                item.ReportBanUntil = reportBanUntil;
                item.IsReportBanned = isReportBanned;
            }

            return comments;
        }




        //建構留言
        public async Task<VMPostCommentItem> BuildCommentViewModelAsync(
    string commentId,
    string? currentMemberId,
    string? currentCreatorId)
        {
            var vm = await _context.PostComments
                .Include(c => c.Member)
                .Where(c => c.CommentID == commentId)
                .Select(c => new VMPostCommentItem
                {
                    CommentID = c.CommentID,
                    PostID = c.PostID,
                    MemberID = c.MemberID,
                    MemberName = c.Member.DisplayName,
                    MemberAvatar = c.Member.ImageUrl,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Status = c.Status,
                    IsOwner = c.MemberID == currentMemberId
                })
                .FirstAsync();

            vm.ReactionButton = await _reactionService.GetButtonStateAsync(
                currentMemberId,
                ReactionTargetType.PostComment,
                vm.CommentID
            );

            if (!string.IsNullOrWhiteSpace(currentMemberId))
            {
                vm.ReportBanUntil = await _context.Members
                    .AsNoTracking()
                    .Where(m => m.MemberID == currentMemberId)
                    .Select(m => m.ReportBanUntil)
                    .FirstOrDefaultAsync();

                vm.IsReportBanned = vm.ReportBanUntil.HasValue &&
                                    vm.ReportBanUntil.Value > DateTime.Now;
            }

            return vm;
        }
    }
}