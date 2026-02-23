using CraftDailyCorner.Services.Interface;

namespace CraftDailyCorner.Services
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

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}

