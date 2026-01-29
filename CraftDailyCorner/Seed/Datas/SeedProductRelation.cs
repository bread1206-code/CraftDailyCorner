using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProductRelation
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProductRelation(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.ProductCategories.Any()) // 避免重複 Seed
            {
                var ProductCategory = new List<ProductCategory>
                {
                    new ProductCategory { ProductID = "P000000001", CategoryID = 8 },
                    new ProductCategory { ProductID = "P000000002", CategoryID = 9 }
                };
                _context.ProductCategories.AddRange(ProductCategory);
                _context.SaveChanges();
            }
            if (!_context.ProductTags.Any()) // 避免重複 Seed
            {
                var ProductTag = new List<ProductTag>
                {
                    new ProductTag { ProductID = "P000000001", TagID = 1 },
                    new ProductTag { ProductID = "P000000001", TagID = 3 },
                    new ProductTag { ProductID = "P000000002", TagID = 1 }
                };
                _context.ProductTags.AddRange(ProductTag);
                _context.SaveChanges();
            }
        }
    }
}
