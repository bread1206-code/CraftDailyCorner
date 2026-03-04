using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CraftDailyCorner.Services
{
    public class AuthService : IAuthService
    {
        private const string Scheme = "CraftDailyCornerLogin";
        private readonly CraftDailyCornerContext _context;

        public AuthService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task SignInMemberAsync(HttpContext httpContext, string memberId)
        {
            var principal = await BuildPrincipalAsync(memberId);
            await httpContext.SignInAsync(Scheme, principal);
        }

        public async Task RefreshSignInAsync(HttpContext httpContext, string memberId)
        {
            // 做法A：SignOut + SignIn（讓 claims 立刻刷新）
            await httpContext.SignOutAsync(Scheme);
            var principal = await BuildPrincipalAsync(memberId);
            await httpContext.SignInAsync(Scheme, principal);
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(Scheme);
        }

        private async Task<ClaimsPrincipal> BuildPrincipalAsync(string memberId)
        {
            // 顯示名稱
            var displayName = await _context.Members.AsNoTracking()
                .Where(m => m.MemberID == memberId)
                .Select(m => m.DisplayName)
                .FirstOrDefaultAsync() ?? "使用者";

            // 角色 RoleID "01"一般會員 "02"創作者 "03"管理者
            var roles = await (
                from mr in _context.MemberRoles.AsNoTracking()
                join r in _context.Roles.AsNoTracking()
                    on mr.RoleID equals r.RoleID
                where mr.MemberID == memberId
                select r.RoleID
            ).Distinct().ToListAsync();

            if (!roles.Any())
                roles.Add("01");

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, memberId),
                new Claim(ClaimTypes.Name, displayName)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // 如果是創作者，加入 CreatorID Claim
            if (roles.Contains("02"))
            {
                var creatorId = await _context.CreatorProfiles.AsNoTracking()
                    .Where(c => c.MemberID == memberId)
                    .Select(c => c.CreatorID)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(creatorId))
                    claims.Add(new Claim("CreatorID", creatorId));
            }

            var identity = new ClaimsIdentity(claims, Scheme);
            return new ClaimsPrincipal(identity);
        }
    }
}