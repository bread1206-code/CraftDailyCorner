using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMemberStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMemberStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.MemberStatuses.Any()) return;

            _context.MemberStatuses.AddRange(
                new MemberStatus { 
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "帳號可正常使用",
                    IsActive = true
                },
                new MemberStatus { StatusID = 2,
                    StatusCode = "Suspended",
                    StatusName = "停權",
                    Description = "帳號遭管理員停權",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }
}
