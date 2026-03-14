using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedProducts
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedProducts(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Products == null || !seedContext.Products.Any())
                throw new Exception("DemoSeedContext.Products 沒有資料");

            var existingProducts = _context.Products
                .Select(x => new { x.ProductID, x.CreatedAt })
                .ToDictionary(x => x.ProductID, x => x.CreatedAt);

            var creatorConfirmedAtMap = seedContext.CreatorConfirmedAtMap;
            var productCreatedAtMap = seedContext.ProductCreatedAtMap;

            var products = new List<Product>();

            foreach (var row in seedContext.Products)
            {
                if (existingProducts.TryGetValue(row.ProductID, out var existingCreatedAt))
                {
                    productCreatedAtMap[row.ProductID] = existingCreatedAt;
                    continue;
                }

                if (!creatorConfirmedAtMap.TryGetValue(row.CreatorID, out var creatorConfirmedAt))
                    throw new Exception($"找不到 CreatorConfirmedAt：{row.CreatorID}");

                var createdAt = DemoSeedTimeHelper.GetProductCreatedAt(
                    creatorConfirmedAt,
                    row.SortOrder);

                products.Add(new Product
                {
                    ProductID = row.ProductID,
                    ProductName = row.ProductName,
                    Description = row.Description,
                    Price = row.Price,
                    CostPrice = row.CostPrice,
                    StatusID = row.StatusID,
                    CreatedAt = createdAt,
                    CreatorID = row.CreatorID
                });

                productCreatedAtMap[row.ProductID] = createdAt;
            }

            if (products.Any())
            {
                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }
    }
}