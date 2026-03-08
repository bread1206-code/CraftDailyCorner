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
                        Description = "可以使用一般功能。"
                    }, new Role
                    {
                        RoleID = "02",
                        RoleName = "創作者",
                        Description = "可以使用販賣功能"
                    }, new Role
                    {
                        RoleID = "03",
                        RoleName = "管理者",
                        Description = "負責管理平台事務。"
                    }, new Role
                    {
                        RoleID = "04",
                        RoleName = "超級管理者",
                        Description = "可以變更平台設定、給予管理者身分。"
                    }
                };
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }
    }
}
