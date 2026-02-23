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
                    Description = "作品集正常啟用",
                    IsActive = true
                },
                new PortfolioStatus
                {
                    StatusID = 2,
                    StatusCode = "Suspended",
                    StatusName = "停權",
                    Description = "作品集停權",
                    IsActive = false
                },
                new PortfolioStatus
                {
                    StatusID = 3,
                    StatusCode = "Deleted",
                    StatusName = "已刪除",
                    Description = "作品集已刪除",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }
}

