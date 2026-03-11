using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPostService : ICreatorPostService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly IReactionService _reactionService;
        private readonly INotificationService _notificationService;

        public CreatorPostService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService,
            IReactionService reactionService,
            INotificationService notificationService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _reactionService = reactionService;
            _notificationService = notificationService;
        }

        // ===============================
        // 前台列表（公開 + 搜尋 + 分頁）
        // ===============================
        public async Task<VMPostIndex> GetPostIndexAsync(VMPostIndexQuery query)
        {
            var baseQuery = _context.CreatorPosts
                .Where(p =>
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                baseQuery = baseQuery.Where(p =>
                    p.Title.Contains(query.Keyword) ||
                    p.Content.Contains(query.Keyword));
            }

            var totalCount = await baseQuery.CountAsync();

            var posts = await baseQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new VMPostListItem
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    CreatorID = p.CreatorProfile.CreatorID,
                    BrandName = p.CreatorProfile.BrandName,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CommentCount = p.PostComments
                        .Count(c => c.Status == PostCommentStatus.Visible),

                    ReactionSummary = new CraftDailyCorner.ViewModels.Reaction.VMReactionButton
                    {
                        TargetType = ReactionTargetType.CreatorPost,
                        TargetID = p.PostID,

                        TotalCount = _context.Reactions
                            .Count(r => r.TargetType == ReactionTargetType.CreatorPost && r.TargetID == p.PostID),

                        TopReactionType = _context.Reactions
                            .Where(r => r.TargetType == ReactionTargetType.CreatorPost && r.TargetID == p.PostID)
                            .GroupBy(r => r.ReactionType)
                            .OrderByDescending(g => g.Count())
                            .ThenBy(g => (byte)g.Key)
                            .Select(g => (ReactionType?)g.Key)
                            .FirstOrDefault()
                    },
                    Preview = p.Content
                })
                .ToListAsync();

            return new VMPostIndex
            {
                Query = query,
                Posts = posts,
                TotalCount = totalCount,
                CreatorID = posts.FirstOrDefault()?.CreatorID ?? "",
                BrandName = posts.FirstOrDefault()?.BrandName ?? ""
            };
        }

        // ===============================
        // 前台單篇
        // ===============================
        public async Task<VMPostDetail?> GetPostDetailAsync(
            string postId,
            string? currentMemberId)
        {
            var post = await _context.CreatorPosts
                .Where(p => p.PostID == postId && p.StatusID == 1)
                .Select(p => new
                {
                    p.PostID,
                    p.Title,
                    p.Content,
                    p.ImageUrl,
                    CreatorName = p.CreatorProfile.BrandName,
                    p.CreatedAt,
                    p.UpdatedAt,
                    OwnerId = p.CreatorProfile.MemberID,
                    p.CreatorID
                })
                .FirstOrDefaultAsync();

            if (post == null) return null;

            var reactionVm = await _reactionService
                .GetButtonStateAsync(
                    currentMemberId,
                    ReactionTargetType.CreatorPost,
                    post.PostID);

            bool isReportBanned = false;
            DateTime? reportBanUntil = null;

            if (!string.IsNullOrWhiteSpace(currentMemberId))
            {
                var member = await _context.Members
                    .AsNoTracking()
                    .Where(m => m.MemberID == currentMemberId)
                    .Select(m => new
                    {
                        m.ReportBanUntil
                    })
                    .FirstOrDefaultAsync();

                if (member != null)
                {
                    reportBanUntil = member.ReportBanUntil;
                    isReportBanned = member.ReportBanUntil.HasValue &&
                                     member.ReportBanUntil.Value > DateTime.Now;
                }
            }

            return new VMPostDetail
            {
                PostID = post.PostID,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CreatorName = post.CreatorName,
                CreatorID = post.CreatorID,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                IsOwner = currentMemberId != null && post.OwnerId == currentMemberId,
                ReactionButton = reactionVm,

                IsReportBanned = isReportBanned,
                ReportBanUntil = reportBanUntil
            };
        }

        // ===============================
        // 權限判斷
        // ===============================
        public async Task<bool> CanViewPostAsync(string postId, string? memberId)
        {
            var post = await _context.CreatorPosts
                .Include(c => c.CreatorProfile)
                .FirstOrDefaultAsync(p =>
                    p.PostID == postId &&
                    p.StatusID == 1);

            if (post == null)
                return false;

            if (post.Visibility == CreatorPostVisibility.Public)
                return true;

            // 創作者自己可看
            if (memberId != null && post.CreatorProfile.MemberID == memberId)
                return true;

            if (post.Visibility == CreatorPostVisibility.Private)
                return false;

            if (post.Visibility == CreatorPostVisibility.Followers)
            {
                if (memberId == null)
                    return false;

                return await _context.FollowCreators
                    .AnyAsync(f =>
                        f.CreatorID == post.CreatorID &&
                        f.MemberID == memberId);
            }

            return false;
        }

        // ===============================
        // 後台列表
        // ===============================
        public async Task<List<VMPostListItem>>
            GetCreatorPostsAsync(string creatorId)
        {
            return await _context.CreatorPosts
                .Where(p =>
                    p.CreatorID == creatorId &&
                    p.StatusID != 3)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMPostListItem
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CommentCount = p.PostComments
                        .Count(c => c.Status == PostCommentStatus.Visible)
                })
                .ToListAsync();
        }

        // ===============================
        // 建立
        // ===============================
        public async Task CreateAsync(
            CreateCreatorPostDTO dto,
            string creatorId)
        {
            if (dto.ImageFile == null)
                throw new Exception("請上傳封面圖片");

            var postId = Guid.NewGuid().ToString();
            var now = DateTime.Now;

            var imageKey = _imageUploadService.UploadImage(
                dto.ImageFile,
                null,
                "05CreatorPost",
                ImageSizePresets.Post,
                postId
            );

            var post = new CreatorPost
            {
                PostID = postId,
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = postId,
                Visibility = dto.Visibility,
                CreatorID = creatorId,
                StatusID = 1,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.CreatorPosts.Add(post);
            await _context.SaveChangesAsync();

            // ===== 第五階段：創作者新日誌通知 =====
            // 規格：建立新日誌時通知追蹤者（下架再上架不算）
            // 所以只在 Create 時送
            if (post.Visibility == CreatorPostVisibility.Public)
            {
                var followerMemberIds = await _context.FollowCreators
                    .Where(x => x.CreatorID == creatorId)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (followerMemberIds.Any())
                {
                    var dtos = followerMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.CreatorNewPost,
                        Title = "創作者新日誌通知",
                        Content = $"你追蹤的創作者發布了新日誌「{post.Title}」。",
                        LinkUrl = $"/Post/Detail/{post.PostID}",
                        RelatedEntityType = "Post",
                        RelatedEntityId = post.PostID
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }
        }

        // ===============================
        // 更新
        // ===============================
        public async Task UpdateAsync(
            UpdateCreatorPostDTO dto,
            string creatorId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == dto.PostID &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (post == null)
                throw new Exception("找不到日誌或無權限");

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.Visibility = dto.Visibility;
            post.UpdatedAt = DateTime.Now;

            if (dto.NewImageFile != null)
            {
                var imageKey = _imageUploadService.UploadImage(
                    dto.NewImageFile,
                    null,
                    "05CreatorPost",
                    ImageSizePresets.Post,
                    dto.PostID
                );

                post.ImageUrl = dto.PostID;
            }

            await _context.SaveChangesAsync();
        }

        // ===============================
        // 軟刪除
        // ===============================
        public async Task SoftDeleteAsync(
            string postId,
            string creatorId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == postId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (post == null)
                throw new Exception("找不到日誌或無權限");

            post.StatusID = 3;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId)
        {
            return await _context.CreatorPosts
                .Where(p =>
                    p.PostID == postId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1)
                .Select(p => new VMCreatorPostEdit
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    Content = p.Content,
                    Visibility = p.Visibility,
                    CurrentImageUrl = p.ImageUrl,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}