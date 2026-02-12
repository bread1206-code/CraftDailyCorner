using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPortfolioStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPortfolioStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PortfolioStatuses.Any()) return;

            _context.PortfolioStatuses.AddRange(
                new PortfolioStatus
                {
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "創作者正常啟用",
                    IsActive = true
                },
                new PortfolioStatus
                {
                    StatusID = 2,
                    StatusCode = "Suspended",
                    StatusName = "停權",
                    Description = "創作者帳號停權",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }
}

