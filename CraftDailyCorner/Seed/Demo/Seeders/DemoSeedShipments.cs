using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedShipments
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedShipments(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Shipments == null || !seedContext.Shipments.Any())
                throw new Exception("DemoSeedContext.Shipments 沒有資料");

            var existingOrderIds = _context.Orders
                .Select(x => x.OrderID)
                .ToHashSet();

            var existingShipmentOrderIds = _context.Shipments
                .Select(x => x.OrderID)
                .ToHashSet();

            var shipments = new List<Shipment>();

            foreach (var row in seedContext.Shipments)
            {
                if (!existingOrderIds.Contains(row.OrderID))
                    throw new Exception($"Shipments.csv 找不到對應 OrderID：{row.OrderID}");

                // 目前 Demo 資料設計是一張訂單一筆物流紀錄
                if (existingShipmentOrderIds.Contains(row.OrderID))
                    continue;

                shipments.Add(new Shipment
                {
                    // ShipmentID 先不手動指定，讓資料庫自行處理
                    TrackingNo = row.TrackingNo,
                    StatusID = row.StatusID,
                    ShippedAt = row.ShippedAt,
                    DeliveredAt = row.DeliveredAt,
                    OrderID = row.OrderID
                });
            }

            if (shipments.Any())
            {
                _context.Shipments.AddRange(shipments);
                _context.SaveChanges();
            }
        }
    }
}