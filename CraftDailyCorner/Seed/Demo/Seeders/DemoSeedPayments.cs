using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedPayments
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedPayments(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Payments == null || !seedContext.Payments.Any())
                throw new Exception("DemoSeedContext.Payments 沒有資料");

            var existingOrderIds = _context.Orders
                .Select(x => x.OrderID)
                .ToHashSet();

            var existingPaymentOrderIds = _context.Payments
                .Select(x => x.OrderID)
                .ToHashSet();

            var payments = new List<Payment>();

            foreach (var row in seedContext.Payments)
            {
                if (!existingOrderIds.Contains(row.OrderID))
                    throw new Exception($"Payments.csv 找不到對應 OrderID：{row.OrderID}");

                // 目前 Demo 資料設計是一張訂單一筆付款紀錄
                if (existingPaymentOrderIds.Contains(row.OrderID))
                    continue;

                payments.Add(new Payment
                {
                    // PaymentID 先不手動指定，讓資料庫自行處理
                    MethodID = row.MethodID,
                    Amount = row.Amount,
                    StatusID = row.StatusID,
                    GatewayTradeNo = row.GatewayTradeNo,
                    AttemptNo = row.AttemptNo,
                    PaidAt = row.PaidAt,
                    CreatedAt = row.CreatedAt,
                    OrderID = row.OrderID
                });
            }

            if (payments.Any())
            {
                _context.Payments.AddRange(payments);
                _context.SaveChanges();
            }
        }
    }
}