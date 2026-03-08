using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMemberRole
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMemberRole(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.MemberRoles.Any()) // 避免重複 Seed
            {
                var memberRoles = new List<MemberRole>
                {
                    new MemberRole
                    {
                        MemberID = "M0000001",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000001",
                        RoleID = "04",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000002",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000003",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000004",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000004",
                        RoleID = "02",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000005",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000005",
                        RoleID = "02",
                        AssignedAt = DateTime.Now
                    }
                };
                _context.MemberRoles.AddRange(memberRoles);
                _context.SaveChanges();
            }
        }
    }
}
