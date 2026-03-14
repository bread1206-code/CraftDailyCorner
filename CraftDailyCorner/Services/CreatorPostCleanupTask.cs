using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPostCleanupTask : ISoftDeleteCleanupTask
    {
        public async Task CleanupAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<CraftDailyCornerContext>();

            var fileService = scope.ServiceProvider
                .GetRequiredService<IImageFileService>();

            var threshold = DateTime.Now.AddDays(-7);

            var posts = await context.CreatorPosts
                .IgnoreQueryFilters()
                .Where(p =>
                    p.StatusID == 3 &&
                    p.UpdatedAt <= threshold)
                .ToListAsync();

            foreach (var post in posts)
            {
                fileService.DeleteCreatorPostImage(post.CreatorID, post.ImageUrl);

                var comments = await context.PostComments
                    .Where(c => c.PostID == post.PostID)
                    .ToListAsync();

                context.PostComments.RemoveRange(comments);
                context.CreatorPosts.Remove(post);
            }

            await context.SaveChangesAsync();
        }
    }
}