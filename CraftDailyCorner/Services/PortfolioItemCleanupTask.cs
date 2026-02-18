using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class PortfolioItemCleanupTask : ISoftDeleteCleanupTask
    {
        public async Task CleanupAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<CraftDailyCornerContext>();

            var fileService = scope.ServiceProvider
                .GetRequiredService<IImageFileService>();

            var threshold = DateTime.Now.AddDays(-7);

            var expiredItems = await context.PortfolioItems
                .IgnoreQueryFilters()
                .Where(i =>
                    i.IsDeleted &&
                    i.DeletedAt != null &&
                    i.DeletedAt < threshold)
                .ToListAsync();

            foreach (var item in expiredItems)
            {
                fileService.DeletePortfolioImage(item.ImageUrl);
                context.PortfolioItems.Remove(item);
            }

            await context.SaveChangesAsync();
        }
    }
}
