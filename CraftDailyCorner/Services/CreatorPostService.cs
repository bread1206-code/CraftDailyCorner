using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPostService : ICreatorPostService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public CreatorPostService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
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
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CommentCount = p.PostComments
                        .Count(c => c.Status == PostCommentStatus.Visible)
                })
                .ToListAsync();

            return new VMPostIndex
            {
                Query = query,
                Posts = posts,
                TotalCount = totalCount
            };
        }

        // ===============================
        // 前台單篇
        // ===============================
        public async Task<VMPostDetail?> GetPostDetailAsync(string postId)
        {
            return await _context.CreatorPosts
                .Where(p =>
                    p.PostID == postId &&
                    p.StatusID == 1)
                .Select(p => new VMPostDetail
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatorName = p.CreatorProfile.DisplayName,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        // ===============================
        // 權限判斷
        // ===============================
        public async Task<bool> CanViewPostAsync(string postId, string? memberId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == postId &&
                    p.StatusID == 1);

            if (post == null)
                return false;

            if (post.Visibility == CreatorPostVisibility.Public)
                return true;

            // 創作者自己可看
            if (memberId != null && post.CreatorID == memberId)
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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.CreatorPosts.Add(post);
                await _context.SaveChangesAsync();
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
        public async Task<VMCreatorPostEdit?> GetEditDataAsync(string postId,string creatorId)
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