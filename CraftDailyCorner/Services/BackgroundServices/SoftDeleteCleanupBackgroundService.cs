using CraftDailyCorner.Services.Interface;

namespace CraftDailyCorner.Services.BackgroundServices
{
    public class SoftDeleteCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SoftDeleteCleanupBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var tasks = scope.ServiceProvider
                    .GetServices<ISoftDeleteCleanupTask>();

                foreach (var task in tasks)
                {
                    await task.CleanupAsync(scope.ServiceProvider);
                }
                //每 24 小時執行一次清理
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}

