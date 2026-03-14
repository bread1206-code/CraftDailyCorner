using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedInventories
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedInventories(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Products == null || !seedContext.Products.Any())
                throw new Exception("DemoSeedContext.Products 沒有資料");

            var existingProductIds = _context.Inventories
                .Select(x => x.ProductID)
                .ToHashSet();

            var productCreatedAtMap = seedContext.ProductCreatedAtMap;

            var inventories = new List<Inventory>();

            foreach (var row in seedContext.Products)
            {
                if (existingProductIds.Contains(row.ProductID))
                    continue;

                if (!productCreatedAtMap.TryGetValue(row.ProductID, out var productCreatedAt))
                    throw new Exception($"找不到 ProductCreatedAt：{row.ProductID}");

                var stockQty = DemoSeedInventoryHelper.GetStockQty(
                    row.ProductID,
                    row.StockLevelType);

                inventories.Add(new Inventory
                {
                    ProductID = row.ProductID,
                    StockQty = stockQty,
                    AlertQty = row.AlertQty,
                    UpdatedAt = productCreatedAt
                });
            }

            if (inventories.Any())
            {
                _context.Inventories.AddRange(inventories);
                _context.SaveChanges();
            }
        }
    }
}