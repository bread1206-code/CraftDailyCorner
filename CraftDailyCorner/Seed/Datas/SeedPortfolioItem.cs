using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPortfolioItem
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPortfolioItem(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.PortfolioItems.Any()) // 避免重複 Seed
            {
                var portfolioItems = new List<PortfolioItem>
                {
                    new PortfolioItem
                    {
                        ItemID = Guid.NewGuid().ToString(),
                        ImageUrl = Guid.NewGuid().ToString(),
                        Title = "榫接木盒",
                        Description = "全手工榫接製作",
                        SortOrder = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PortfolioID = imageGuids[0]
                    }
                };
                _context.PortfolioItems.AddRange(portfolioItems);
                _context.SaveChanges();
            }
        }
    }
}
