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
        public void Run(string[] prtfolioGuids, string[] imageGuids)
        {
            if (!_context.PortfolioItems.Any()) // 避免重複 Seed
            {
                var portfolioItems = new List<PortfolioItem>
                {
                    new PortfolioItem
                    {
                        ImageUrl = imageGuids[0],
                        SortOrder = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PortfolioID = prtfolioGuids[0]
                    },new PortfolioItem
                    {
                        ImageUrl = imageGuids[1],
                        SortOrder = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PortfolioID = prtfolioGuids[0]
                    }
                };
                _context.PortfolioItems.AddRange(portfolioItems);
                _context.SaveChanges();
            }
        }
    }
}
