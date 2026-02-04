using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedRole
    {
        private readonly CraftDailyCornerContext _context;

        public SeedRole(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Roles.Any()) // 避免重複 Seed
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        RoleID = "01",
                        RoleName = "一般會員",
                        Description = "可以使用大部分功能。"
                    }, new Role
                    {
                        RoleID = "02",
                        RoleName = "創作者",
                        Description = "可以使用販賣功能"
                    }, new Role
                    {
                        RoleID = "03",
                        RoleName = "管理者",
                        Description = "可以管理平台資料。"
                    }
                };
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }
    }
}
