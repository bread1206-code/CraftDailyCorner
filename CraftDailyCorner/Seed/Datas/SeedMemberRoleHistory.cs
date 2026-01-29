using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMemberRoleHistory
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMemberRoleHistory(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.MemberRoleHistories.Any()) // 避免重複 Seed
            {
                var memberRoleHistories = new List<MemberRoleHistory>
                {
                    new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000001",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000003",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000004",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000005",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000004",
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000005",
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,
                        OperatorMemberID = null
                    }
                };
                _context.MemberRoleHistories.AddRange(memberRoleHistories);
                _context.SaveChanges();
            }
        }
    }
}
