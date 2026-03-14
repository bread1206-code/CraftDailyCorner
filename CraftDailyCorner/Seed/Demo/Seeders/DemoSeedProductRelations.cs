using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedProductRelations
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedProductRelations(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Products == null || !seedContext.Products.Any())
                throw new Exception("DemoSeedContext.Products 沒有資料");

            SeedProductCategories(seedContext);
            SeedProductTags(seedContext);
        }

        private void SeedProductCategories(DemoSeedContext seedContext)
        {
            var existingProductCategories = _context.ProductCategories
                .Select(x => new { x.ProductID, x.CategoryID })
                .ToHashSet();

            var productCategories = new List<ProductCategory>();

            foreach (var row in seedContext.Products)
            {
                if (string.IsNullOrWhiteSpace(row.CategoryIDs))
                    continue;

                var categoryIds = row.CategoryIDs
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Distinct();

                foreach (var categoryIdText in categoryIds)
                {
                    if (!int.TryParse(categoryIdText, out var categoryId))
                        throw new Exception($"CategoryIDs 格式錯誤：ProductID={row.ProductID}, 值={categoryIdText}");

                    if (existingProductCategories.Contains(new { row.ProductID, CategoryID = categoryId }))
                        continue;

                    productCategories.Add(new ProductCategory
                    {
                        ProductID = row.ProductID,
                        CategoryID = categoryId
                    });
                }
            }

            if (productCategories.Any())
            {
                _context.ProductCategories.AddRange(productCategories);
                _context.SaveChanges();
            }
        }

        private void SeedProductTags(DemoSeedContext seedContext)
        {
            var existingProductTags = _context.ProductTags
                .Select(x => new { x.ProductID, x.TagID })
                .ToHashSet();

            var productTags = new List<ProductTag>();

            foreach (var row in seedContext.Products)
            {
                if (string.IsNullOrWhiteSpace(row.TagIDs))
                    continue;

                var tagIds = row.TagIDs
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Distinct();

                foreach (var tagIdText in tagIds)
                {
                    if (!int.TryParse(tagIdText, out var tagId))
                        throw new Exception($"TagIDs 格式錯誤：ProductID={row.ProductID}, 值={tagIdText}");

                    if (existingProductTags.Contains(new { row.ProductID, TagID = tagId }))
                        continue;

                    productTags.Add(new ProductTag
                    {
                        ProductID = row.ProductID,
                        TagID = tagId
                    });
                }
            }

            if (productTags.Any())
            {
                _context.ProductTags.AddRange(productTags);
                _context.SaveChanges();
            }
        }
    }
}