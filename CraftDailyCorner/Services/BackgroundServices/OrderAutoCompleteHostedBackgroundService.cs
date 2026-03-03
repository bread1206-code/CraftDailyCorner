using CraftDailyCorner.Models;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.BackgroundServices
{
    public class OrderAutoCompleteHostedBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderAutoCompleteHostedBackgroundService> _logger;
        // 每 30 分鐘檢查一次，將已送達超過 3 天的訂單自動標記為完成
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);
        private const int AutoCompleteDays = 3;

        public OrderAutoCompleteHostedBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<OrderAutoCompleteHostedBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //啟動後先跑一次（避免等 30 分鐘）
            await RunOnceSafe(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Interval, stoppingToken);
                await RunOnceSafe(stoppingToken);
            }
        }

        private async Task RunOnceSafe(CancellationToken ct)
        {
            try
            {
                await AutoCompleteOrdersAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AutoCompleteOrders failed.");
            }
        }

        private async Task AutoCompleteOrdersAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CraftDailyCornerContext>();

            var now = DateTime.Now;
            var cutoff = now.AddDays(-AutoCompleteDays);

            // 找出：訂單配送中 + 物流已送達 + 送達日期超過 3 天
            var query = db.Orders
                .Where(o => o.StatusID == 4
                            && o.Shipment != null
                            && o.Shipment.StatusID == 3
                            && o.Shipment.DeliveredAt != null
                            && o.Shipment.DeliveredAt <= cutoff);

            var updated = await query.ExecuteUpdateAsync(setters => setters
                .SetProperty(o => o.StatusID, (byte)5)
                .SetProperty(o => o.UpdatedAt, now)
            , ct);

            if (updated > 0)
                _logger.LogInformation("Auto-completed {Count} orders.", updated);
        }
    }
}