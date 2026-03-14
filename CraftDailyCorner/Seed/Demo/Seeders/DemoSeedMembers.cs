using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedMembers
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedMembers(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            var existingIds = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var members = seedContext.Members
                .Where(x => !existingIds.Contains(x.MemberID))
                .Select(x => new Member
                {
                    MemberID = x.MemberID,
                    ImageUrl = "default",
                    DisplayName = x.DisplayName,
                    StatusID = x.StatusID,
                    MaliciousReportCount = 0,
                    ReportBanUntil = null,
                    ViolationCount = 0,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            if (members.Any())
            {
                _context.Members.AddRange(members);
                _context.SaveChanges();
            }
        }
    }
}