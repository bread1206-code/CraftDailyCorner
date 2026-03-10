using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPlatformAnnouncementStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPlatformAnnouncementStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PlatformAnnouncementStatuses.Any()) return;

            _context.PlatformAnnouncementStatuses.AddRange(
                
                new PlatformAnnouncementStatus
                {
                    StatusID = 1,
                    StatusCode = "Draft",
                    StatusName = "草稿",
                    Description = "公告草稿",
                    IsActive = false
                }, 
                new PlatformAnnouncementStatus
                {
                    StatusID = 2,
                    StatusCode = "Published",
                    StatusName = "已發布",
                    Description = "公告顯示中",
                    IsActive = true
                },
                new PlatformAnnouncementStatus
                {
                    StatusID = 3,
                    StatusCode = "Archived",
                    StatusName = "已封存",
                    Description = "公告已封存",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}
