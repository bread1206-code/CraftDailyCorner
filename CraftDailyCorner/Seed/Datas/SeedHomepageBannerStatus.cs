using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedHomepageBannerStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedHomepageBannerStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.HomepageBannerStatuses.Any()) return;

            _context.HomepageBannerStatuses.AddRange(
                new HomepageBannerStatus
                {
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "輪播圖顯示中",
                    IsActive = true
                },
                new HomepageBannerStatus
                {
                    StatusID = 2,
                    StatusCode = "Inactive",
                    StatusName = "停用",
                    Description = "輪播圖停用",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}
