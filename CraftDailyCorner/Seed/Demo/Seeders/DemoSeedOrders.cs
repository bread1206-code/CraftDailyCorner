using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedOrders
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedOrders(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Orders == null || !seedContext.Orders.Any())
                throw new Exception("DemoSeedContext.Orders 沒有資料");

            var existingOrders = _context.Orders
                .Select(x => new { x.OrderID, x.CreatedAt })
                .ToDictionary(x => x.OrderID, x => x.CreatedAt);

            var existingMemberIds = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var orders = new List<Order>();

            foreach (var row in seedContext.Orders)
            {
                if (existingOrders.TryGetValue(row.OrderID, out var existingCreatedAt))
                {
                    seedContext.OrderCreatedAtMap[row.OrderID] = existingCreatedAt;
                    continue;
                }

                if (!existingMemberIds.Contains(row.MemberID))
                    throw new Exception($"Orders.csv 找不到對應 MemberID：{row.MemberID}");

                orders.Add(new Order
                {
                    OrderID = row.OrderID,
                    ReceiverName = row.ReceiverName,
                    ReceiverPhone = row.ReceiverPhone,
                    ShippingAddress = row.ShippingAddress,
                    CreatedAt = row.CreatedAt,
                    UpdatedAt = row.UpdatedAt,
                    StatusID = row.StatusID,
                    TotalAmount = row.TotalAmount,
                    MemberID = row.MemberID
                });

                seedContext.OrderCreatedAtMap[row.OrderID] = row.CreatedAt;
            }

            if (orders.Any())
            {
                _context.Orders.AddRange(orders);
                _context.SaveChanges();
            }
        }
    }
}