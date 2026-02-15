using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorPostStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorPostStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.CreatorPostStatuses.Any()) return;

            _context.CreatorPostStatuses.AddRange(
                new CreatorPostStatus
                {
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "日誌正常啟用",
                    IsActive = true
                },
                new CreatorPostStatus
                {
                    StatusID = 2,
                    StatusCode = "Suspended",
                    StatusName = "停權",
                    Description = "日誌停權",
                    IsActive = false
                },
                new CreatorPostStatus
                {
                    StatusID = 3,
                    StatusCode = "Deleted",
                    StatusName = "已刪除",
                    Description = "日誌已刪除",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}
