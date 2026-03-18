using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedFavoriteProducts
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedFavoriteProducts(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.FavoriteProducts == null || !seedContext.FavoriteProducts.Any())
                throw new Exception("DemoSeedContext.FavoriteProducts 沒有資料");

            var existingFavoriteKeys = _context.FavoriteProducts
                .Select(x => new { x.MemberID, x.ProductID })
                .ToList()
                .Select(x => $"{x.MemberID}|{x.ProductID}")
                .ToHashSet();

            var existingMembers = _context.Members
                .Select(x => new { x.MemberID, x.CreatedAt })
                .ToDictionary(x => x.MemberID, x => x.CreatedAt);

            var existingProducts = _context.Products
                .Select(x => new { x.ProductID, x.CreatedAt })
                .ToDictionary(x => x.ProductID, x => x.CreatedAt);

            var favorites = new List<FavoriteProduct>();

            int skippedMissingMember = 0;
            int skippedMissingProduct = 0;
            int skippedDuplicate = 0;
            int skippedInvalidTime = 0;

            foreach (var row in seedContext.FavoriteProducts)
            {
                var key = $"{row.MemberID}|{row.ProductID}";

                if (existingFavoriteKeys.Contains(key))
                {
                    skippedDuplicate++;
                    continue;
                }

                if (!existingMembers.TryGetValue(row.MemberID, out var memberCreatedAt))
                {
                    skippedMissingMember++;
                    continue;
                }

                if (!existingProducts.TryGetValue(row.ProductID, out var productCreatedAt))
                {
                    skippedMissingProduct++;
                    continue;
                }

                var minCreatedAt = memberCreatedAt > productCreatedAt
                    ? memberCreatedAt
                    : productCreatedAt;

                if (row.CreatedAt < minCreatedAt)
                {
                    skippedInvalidTime++;
                    continue;
                }

                favorites.Add(new FavoriteProduct
                {
                    MemberID = row.MemberID,
                    ProductID = row.ProductID,
                    CreatedAt = row.CreatedAt
                });

                existingFavoriteKeys.Add(key);
            }

            if (favorites.Any())
            {
                _context.FavoriteProducts.AddRange(favorites);
                _context.SaveChanges();
            }

            Console.WriteLine("=== DemoSeedFavoriteProducts ===");
            Console.WriteLine($"新增 FavoriteProducts：{favorites.Count}");
            Console.WriteLine($"略過 - 找不到 Member：{skippedMissingMember}");
            Console.WriteLine($"略過 - 找不到 Product：{skippedMissingProduct}");
            Console.WriteLine($"略過 - 重複資料：{skippedDuplicate}");
            Console.WriteLine($"略過 - CreatedAt 不合法：{skippedInvalidTime}");
        }
    }
}