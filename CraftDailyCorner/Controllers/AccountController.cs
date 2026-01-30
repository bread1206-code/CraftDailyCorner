using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    public class AccountController : Controller
    {
        private readonly CraftDailyCornerContext _context;

        public AccountController(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMLogin login)
        {
            var user = _context.Privacies.FirstOrDefault(u => (u.Email == login.Account || u.Phone == login.Account)&& u.PasswordHash == login.Password);
            if (user != null)
            {
                var roleName = (
                    from mr in _context.MemberRoles
                    join r in _context.Roles on mr.RoleID equals r.RoleID
                    where mr.MemberID == user.MemberID
                    orderby mr.AssignedAt descending
                    select r.RoleName
                ).FirstOrDefault() ?? "未知";

                var DisplayName = _context.Members
                    .Where(m => m.MemberID == user.MemberID)
                    .Select(m => m.DisplayName)
                    .FirstOrDefault() ?? "使用者";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.MemberID.ToString()),
                    new Claim(ClaimTypes.Name,DisplayName),
                    new Claim(ClaimTypes.Role, roleName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "CraftDailyCornerLogin");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                if (login.RememberAccount)
                {
                    Response.Cookies.Append(
                        "RememberAccount",
                        login.Account,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(30),
                            HttpOnly = true
                        }
                    );
                }
                else
                {
                    Response.Cookies.Delete("RememberAccount");
                }
                await HttpContext.SignInAsync("CraftDailyCornerLogin", claimsPrincipal);

                return RedirectToAction("Index", "Home");
            }
            ViewData["ErrorMessage"] = "帳號或密碼錯誤，請重新輸入";
            return View(login);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CraftDailyCornerLogin"); //清除 Cookie
            return RedirectToAction("Index", "Home");
        }
    }
}
