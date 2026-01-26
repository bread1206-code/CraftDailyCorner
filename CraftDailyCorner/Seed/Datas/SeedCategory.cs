using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCategory
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCategory(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Category.Any()) // 避免重複 Seed
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "木作" },
                    new Category { CategoryName = "書法" },
                    new Category { CategoryName = "生活擺飾" }
                };
                _context.Category.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}
