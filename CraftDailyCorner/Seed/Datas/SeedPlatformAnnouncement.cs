using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPlatformAnnouncement
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPlatformAnnouncement(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PlatformAnnouncements.Any())
                return;

            var now = DateTime.Now;

            var platformAnnouncements = new List<PlatformAnnouncement>
            {
                new PlatformAnnouncement
                {
                    Title = "平台正式上線",
                    Content = "歡迎加入手作市集平台！",
                    StatusID = 2, // Active
                    AudienceType = AnnouncementAudienceType.AllMembers,
                    CreatedAt = now,
                    CreatedBy = "M0000001",
                    UpdatedAt = now,
                    UpdatedBy = "M0000001",
                    PublishedAt = now
                },
                new PlatformAnnouncement
                {
                    Title = "春節出貨公告",
                    Content = "春節期間出貨將延後 3–5 日。",
                    StatusID = 2, // Active
                    AudienceType = AnnouncementAudienceType.AllMembers,
                    CreatedAt = now,
                    CreatedBy = "M0000001",
                    UpdatedAt = now,
                    UpdatedBy = "M0000001",
                    PublishedAt = now
                },
                new PlatformAnnouncement
                {
                    Title = "創作者專區功能更新",
                    Content = "創作者中心已新增更多管理功能，歡迎前往查看。",
                    StatusID = 2, // Active
                    AudienceType = AnnouncementAudienceType.CreatorsOnly,
                    CreatedAt = now,
                    CreatedBy = "M0000001",
                    UpdatedAt = now,
                    UpdatedBy = "M0000001",
                    PublishedAt = now
                },
                new PlatformAnnouncement
                {
                    Title = "後台維護公告",
                    Content = "管理後台將於今晚進行例行維護，請管理者提早完成作業。",
                    StatusID = 1, // Draft
                    AudienceType = AnnouncementAudienceType.AdminsOnly,
                    CreatedAt = now,
                    CreatedBy = "M0000001",
                    UpdatedAt = now,
                    UpdatedBy = "M0000001",
                    PublishedAt = null
                }
            };

            _context.PlatformAnnouncements.AddRange(platformAnnouncements);
            _context.SaveChanges();
        }
    }
}