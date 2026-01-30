using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    public class AccountController : Controller
    {
        private readonly CraftDailyCornerContext _context;
        private readonly MemberService _memberService;

        public AccountController(CraftDailyCornerContext context, MemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMLogin login)
        {

            var user = _context.Privacies.FirstOrDefault(u => u.Email == login.Account || u.Phone == login.Account);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "帳號或密碼錯誤，請重新輸入";
                return View(login);
            }
            var hasher = new PasswordHasher<Privacy>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ViewData["ErrorMessage"] = "帳號或密碼錯誤，請重新輸入";
                return View(login);
            }

                var roleName = (
                    from mr in _context.MemberRoles.AsNoTracking()
                    join r in _context.Roles.AsNoTracking() on mr.RoleID equals r.RoleID
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
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CraftDailyCornerLogin"); //清除 Cookie
            return RedirectToAction("Index", "Home");
        }


        //註冊功能
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VMRegister vm)
        {
            // 1. 伺服器端驗證
            if (!ModelState.IsValid)
                return View(vm);
            // 2. 檢查 Email 或 Phone 是否已存在
            if (_context.Privacies.Any(p => p.Email == vm.Email || p.Phone == vm.Phone))
            {
                ModelState.AddModelError("", "Email 或手機號碼已註冊");
                return View(vm);
            }
            // 3. 呼叫 MemberService 進行註冊
            string newMemberId = await _memberService.RegisterMemberAsync(vm);

            TempData["SuccessMessage"] = "註冊成功！請登入";

            return RedirectToAction( "Login","Account");
        }

        //忘記密碼

    }
}
