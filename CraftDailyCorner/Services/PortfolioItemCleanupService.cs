using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CraftDailyCorner.Services
{
    public class PortfolioItemCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PortfolioItemCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupAsync();

                // 每24小時跑一次
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CleanupAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<CraftDailyCornerContext>();

            var fileService = scope.ServiceProvider
                .GetRequiredService<IImageFileService>();

            var threshold = DateTime.UtcNow.AddDays(-7);

            // 注意：要忽略 QueryFilter 才撈得到 IsDeleted 資料
            var expiredItems = await context.PortfolioItems
                .IgnoreQueryFilters()
                .Where(i =>
                    i.IsDeleted &&
                    i.DeletedAt != null &&
                    i.DeletedAt < threshold)
                .ToListAsync();

            foreach (var item in expiredItems)
            {
                // 刪除實體檔案
                fileService.DeletePortfolioImage(item.ImageUrl);

                // 真正刪除資料
                context.PortfolioItems.Remove(item);
            }

            await context.SaveChangesAsync();
        }
    }
}
