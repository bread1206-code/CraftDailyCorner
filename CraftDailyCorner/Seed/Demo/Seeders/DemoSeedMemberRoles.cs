using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedMemberRoles
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedMemberRoles(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            if (_context.MemberRoles.Any())
                return;

            var memberRoles = new List<MemberRole>();

            foreach (var row in seedContext.Members)
            {
                var roleIds = DemoSeedRoleHelper.GetRoleIds(row);

                foreach (var roleId in roleIds)
                {
                    memberRoles.Add(new MemberRole
                    {
                        MemberID = row.MemberID,
                        RoleID = roleId
                    });
                }
            }

            _context.MemberRoles.AddRange(memberRoles);
            _context.SaveChanges();
        }
    }
}