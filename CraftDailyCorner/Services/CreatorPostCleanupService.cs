using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorPostCleanupService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageFileService _imageFileService;

        public CreatorPostCleanupService(
            CraftDailyCornerContext context,
            IImageFileService imageFileService)
        {
            _context = context;
            _imageFileService = imageFileService;
        }

        public async Task CleanupDeletedPostsAsync()
        {
            var threshold = DateTime.Now.AddDays(-7);

            var posts = await _context.CreatorPosts
                .Where(p =>
                    p.StatusID == 3 &&
                    p.UpdatedAt <= threshold)
                .ToListAsync();

            foreach (var post in posts)
            {
                _imageFileService.DeleteCreatorPostImage(post.ImageUrl);

                var comments = await _context.PostComments
                    .Where(c => c.PostID == post.PostID)
                    .ToListAsync();

                _context.PostComments.RemoveRange(comments);
                _context.CreatorPosts.Remove(post);
            }

            await _context.SaveChangesAsync();
        }
    }
}