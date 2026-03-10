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
            if (_context.PlatformAnnouncementStatuses.Any())
                return;

            var statuses = new List<PlatformAnnouncementStatus>
            {
                new PlatformAnnouncementStatus
                {
                    StatusID = 1,
                    StatusCode = "Draft",
                    StatusName = "草稿",
                    Description = "公告尚未發布",
                    IsActive = true
                },
                new PlatformAnnouncementStatus
                {
                    StatusID = 2,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "公告顯示中",
                    IsActive = true
                },
                new PlatformAnnouncementStatus
                {
                    StatusID = 3,
                    StatusCode = "Inactive",
                    StatusName = "停用",
                    Description = "公告已停用，不顯示",
                    IsActive = false
                }
            };

            _context.PlatformAnnouncementStatuses.AddRange(statuses);
            _context.SaveChanges();
        }
    }
}