using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using CraftDailyCorner.ViewModels.CreatorPost.Front;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPostService : ICreatorPostService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorPostService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        //取得後台日誌列表
        public async Task<List<VMCreatorPostListItem>> GetCreatorPostsAsync(string creatorId)
        {
            return await _context.CreatorPosts
                .Where(p => p.CreatorID == creatorId && p.StatusID == 1)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMCreatorPostListItem
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Visibility = p.Visibility,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CommentCount = p.PostComments.Count()
                })
                .ToListAsync();
        }

        //取得單筆編輯資料
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
                    CurrentImageUrl = p.ImageUrl,
                    Visibility = p.Visibility,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        //建立日誌
        public async Task CreateAsync(CreateCreatorPostDTO dto, string creatorId)
        {
            var entity = new CreatorPost
            {
                PostID = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                Visibility = dto.Visibility,
                StatusID = 1,//顯示
                CreatorID = creatorId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.CreatorPosts.Add(entity);
            await _context.SaveChangesAsync();
        }

        //更新日誌
        public async Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId)
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
            post.ImageUrl = dto.ImageUrl;
            post.Visibility = dto.Visibility;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        //軟刪除
        public async Task SoftDeleteAsync(string postId, string creatorId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == postId &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 1);

            if (post == null)
                throw new Exception("找不到日誌或無權限");

            post.StatusID = 2;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<VMPostIndex> GetPostIndexAsync(VMPostIndexQuery query)
        {
            var baseQuery = _context.CreatorPosts
                .Where(p =>
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public);

            // 搜尋
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
                .Select(p => new VMCreatorPostPublicListItem
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    CreatorName = p.CreatorProfile.DisplayName
                })
                .ToListAsync();

            return new VMPostIndex
            {
                Query = query,
                Posts = posts,
                TotalCount = totalCount
            };
        }
        public async Task<VMPostDetail?> GetPublicPostDetailAsync(string postId)
        {
            return await _context.CreatorPosts
                .Where(p =>
                    p.PostID == postId &&
                    p.StatusID == 1 &&
                    p.Visibility == CreatorPostVisibility.Public)
                .Select(p => new VMPostDetail
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CreatorName = p.CreatorProfile.DisplayName
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> CanViewPostAsync(string postId, string? memberId)
        {
            var post = await _context.CreatorPosts
                .Select(p => new
                {
                    p.PostID,
                    p.CreatorID,
                    p.Visibility,
                    p.StatusID
                })
                .FirstOrDefaultAsync(p => p.PostID == postId);

            if (post == null || post.StatusID != 1)
                return false;

            // 公開
            if (post.Visibility == CreatorPostVisibility.Public)
                return true;

            // 未登入不能看 Followers / Private
            if (string.IsNullOrEmpty(memberId))
                return false;

            // 追蹤者可看
            if (post.Visibility == CreatorPostVisibility.Followers)
            {
                return await _context.FollowCreators
                    .AnyAsync(f =>
                        f.CreatorID == post.CreatorID &&
                        f.MemberID == memberId);
            }

            // Private：只有創作者本人
            var creatorId = await _context.CreatorProfiles
                .Where(c => c.MemberID == memberId)
                .Select(c => c.CreatorID)
                .FirstOrDefaultAsync();

            return creatorId == post.CreatorID;
        }

        public async Task<VMPostDetail?> GetPostDetailAsync(string id)
        {
            return await _context.CreatorPosts
                .Where(p => p.PostID == id && p.StatusID == 1)
                .Select(p => new VMPostDetail
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    CreatorName = p.CreatorProfile.DisplayName
                })
                .FirstOrDefaultAsync();
        }
    }
}