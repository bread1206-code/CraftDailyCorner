using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedMemberRoleHistories
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedMemberRoleHistories(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            var existingHistories = _context.MemberRoleHistories
                .Select(x => new { x.MemberID, x.RoleID })
                .ToHashSet();

            var histories = new List<MemberRoleHistory>();

            foreach (var row in seedContext.Members)
            {
                var roleIds = DemoSeedRoleHelper.GetRoleIds(row);

                foreach (var roleId in roleIds)
                {
                    if (existingHistories.Contains(new { row.MemberID, RoleID = roleId }))
                        continue;

                    histories.Add(new MemberRoleHistory
                    {
                        MemberID = row.MemberID,
                        RoleID = roleId,
                        Action = roleId == "01"
                            ? MemberRoleHistoryAction.Created
                            : MemberRoleHistoryAction.Updated,

                        OperatedBy = roleId == "02"
                            ? MemberRoleHistoryOperated.Admin
                            : MemberRoleHistoryOperated.System,

                        OperatorMemberID = roleId == "02"
                            ? "M0000000"
                            : null,

                        OperatedAt = row.CreatedAt
                    });
                }
            }

            if (histories.Any())
            {
                _context.MemberRoleHistories.AddRange(histories);
                _context.SaveChanges();
            }
        }
    }
}