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
            var user = _context.Privacies.FirstOrDefault(u => u.Email == login.Account || u.Phone == login.Account && u.PasswordHash == login.Password);
            var roleName = (
                from p in _context.Privacies
                join mr in _context.MemberRoles on p.MemberID equals mr.MemberID
                join r in _context.Roles on mr.RoleID equals r.RoleID
                where p.Email == login.Account
                || p.Phone == login.Account
                && p.PasswordHash == login.Password
                orderby mr.AssignedAt descending
                select r.RoleName
            ).FirstOrDefault() ?? "未知";
            var DisplayName = (
                from p in _context.Privacies
                join mr in _context.MemberRoles on p.MemberID equals mr.MemberID
                join r in _context.Roles on mr.RoleID equals r.RoleID
                join m in _context.Members on p.MemberID equals m.MemberID
                where p.Email == login.Account
                || p.Phone == login.Account
                && p.PasswordHash == login.Password
                orderby mr.AssignedAt descending
                select m.DisplayName
            ).FirstOrDefault() ?? "使用者";

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,DisplayName),
                    new Claim(ClaimTypes.Role, roleName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "CraftDailyCornerLogin");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
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
