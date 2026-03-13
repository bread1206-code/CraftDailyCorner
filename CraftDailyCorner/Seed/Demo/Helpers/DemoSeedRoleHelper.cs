using CraftDailyCorner.Seed.Demo.Sources;

namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedRoleHelper
    {
        public static List<string> GetRoleIds(MemberSeedRow row)
        {
            var roleIds = new List<string> { "01" }; // 所有人都有一般會員

            if (row.IsCreator)
            {
                roleIds.Add("02");
            }

            if (row.IsAdmin)
            {
                if (string.Equals(row.AdminLevel, "admin", StringComparison.OrdinalIgnoreCase))
                {
                    roleIds.Add("03");
                }
                else if (string.Equals(row.AdminLevel, "super", StringComparison.OrdinalIgnoreCase))
                {
                    roleIds.Add("04");
                }
            }

            return roleIds;
        }
    }
}