using System.Security.Claims;

namespace CraftDailyCorner.Extensions
{
    public static class UserExtensions
    {
        public static string GetMemberId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("找不到 MemberID");
        }

        public static string GetCreatorId(this ClaimsPrincipal user)
        {
            return user.FindFirst("CreatorID")?.Value
                ?? throw new Exception("找不到 CreatorID");
        }
    }
}
