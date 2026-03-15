using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedOrderDetails
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedOrderDetails(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.OrderDetails == null || !seedContext.OrderDetails.Any())
                throw new Exception("DemoSeedContext.OrderDetails 沒有資料");

            var existingOrderDetails = _context.OrderDetails
                .Select(x => new { x.OrderID, x.ProductID })
                .ToHashSet();

            var existingOrderIds = _context.Orders
                .Select(x => x.OrderID)
                .ToHashSet();

            var existingProductIds = _context.Products
                .Select(x => x.ProductID)
                .ToHashSet();

            var orderDetails = new List<OrderDetail>();

            foreach (var row in seedContext.OrderDetails)
            {
                if (!existingOrderIds.Contains(row.OrderID))
                    throw new Exception($"OrderDetails.csv 找不到對應 OrderID：{row.OrderID}");

                if (!existingProductIds.Contains(row.ProductID))
                    throw new Exception($"OrderDetails.csv 找不到對應 ProductID：{row.ProductID}");

                var key = new
                {
                    row.OrderID,
                    row.ProductID
                };

                if (existingOrderDetails.Contains(key))
                    continue;

                orderDetails.Add(new OrderDetail
                {
                    OrderID = row.OrderID,
                    ProductID = row.ProductID,
                    ProductNameSnapshot = row.ProductNameSnapshot,
                    PriceSnapshot = row.PriceSnapshot,
                    CostSnapshot = row.CostSnapshot,
                    Quantity = row.Quantity
                });
            }

            if (orderDetails.Any())
            {
                _context.OrderDetails.AddRange(orderDetails);
                _context.SaveChanges();
            }
        }
    }
}