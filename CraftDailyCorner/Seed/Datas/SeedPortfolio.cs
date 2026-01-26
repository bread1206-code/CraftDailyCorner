using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPortfolio
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPortfolio(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.Portfolio.Any()) // 避免重複 Seed
            {
                var portfolios = new List<Portfolio>
                {
                    new Portfolio
                    {
                        PortfolioID = imageGuids[0],
                        Title = "木作精選",
                        Description = "近年代表作品",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    }
                };
                _context.Portfolio.AddRange(portfolios);
                _context.SaveChanges();
            }
        }
    }
}
