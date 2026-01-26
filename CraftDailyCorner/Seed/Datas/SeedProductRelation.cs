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
            if (!_context.ProductCategory.Any()) // 避免重複 Seed
            {
                var ProductCategory = new List<ProductCategory>
                {
                    new ProductCategory { ProductID = "P000000001", CategoryID = 1 },
                    new ProductCategory { ProductID = "P000000002", CategoryID = 2 }
                };
                _context.ProductCategory.AddRange(ProductCategory);
                _context.SaveChanges();
            }
            if (!_context.ProductTag.Any()) // 避免重複 Seed
            {
                var ProductTag = new List<ProductTag>
                {
                    new ProductTag { ProductID = "P000000001", TagID = 1 },
                    new ProductTag { ProductID = "P000000001", TagID = 3 },
                    new ProductTag { ProductID = "P000000002", TagID = 1 }
                };
                _context.ProductTag.AddRange(ProductTag);
                _context.SaveChanges();
            }
        }
    }
}
