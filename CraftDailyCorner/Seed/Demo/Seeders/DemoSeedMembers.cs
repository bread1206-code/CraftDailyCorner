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

            if (_context.Members.Any())
                return;

            var members = seedContext.Members.Select(row => new Member
            {
                MemberID = row.MemberID,
                ImageUrl = "default",
                DisplayName = row.DisplayName,
                StatusID = row.StatusID,
                MaliciousReportCount = 0,
                ReportBanUntil = null,
                ViolationCount = 0,
                CreatedAt = row.CreatedAt
            }).ToList();

            _context.Members.AddRange(members);
            _context.SaveChanges();
        }
    }
}