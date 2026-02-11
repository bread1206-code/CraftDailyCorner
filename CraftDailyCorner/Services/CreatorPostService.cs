using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Front.CreatorPost;
using CraftDailyCorner.ViewModels.Front.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPostService : ICreatorPostService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorPostService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<List<VMCreatorPostListItem>> GetCreatorPostsAsync(string creatorId)
        {
            return await _context.CreatorPosts
                .Where(p => p.CreatorID == creatorId && p.StatusID == 0)
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

        public async Task<VMCreatorPostEdit?> GetEditDataAsync(string postId, string creatorId)
        {
            return await _context.CreatorPosts
                .Where(p => p.PostID == postId
                         && p.CreatorID == creatorId
                         && p.StatusID == 0)
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

        public async Task CreateAsync(CreateCreatorPostDTO dto, string creatorId)
        {
            var post = new CreatorPost
            {
                PostID = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                Visibility = dto.Visibility,
                StatusID = 0,
                CreatorID = creatorId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.CreatorPosts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateCreatorPostDTO dto, string creatorId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == dto.PostID &&
                    p.CreatorID == creatorId &&
                    p.StatusID == 0);

            if (post == null)
                throw new Exception("找不到日誌");

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.ImageUrl = dto.ImageUrl;
            post.Visibility = dto.Visibility;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string postId, string creatorId)
        {
            var post = await _context.CreatorPosts
                .FirstOrDefaultAsync(p =>
                    p.PostID == postId &&
                    p.CreatorID == creatorId);

            if (post == null)
                throw new Exception("找不到日誌");

            post.StatusID = 1;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public Task SoftDeleteAsync(string postId, string creatorId)
        {
            throw new NotImplementedException();
        }
    }
}
