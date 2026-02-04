using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
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
        private readonly CartService _cartService;

        public AccountController(CraftDailyCornerContext context, MemberService memberService,CartService cartService)
        {
            _context = context;
            _memberService = memberService;
            _cartService = cartService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMLogin login, string? returnUrl)
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
                    orderby mr.RoleID descending
                    select r.RoleName
                ).FirstOrDefault() ?? "未知"; //取得最大權限的角色(orderby)

                var DisplayName = _context.Members
                    .Where(m => m.MemberID == user.MemberID)
                    .Select(m => m.DisplayName)
                    .FirstOrDefault() ?? "使用者";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.MemberID.ToString()),
                    new Claim(ClaimTypes.Name,DisplayName),//會員暱稱
                    new Claim(ClaimTypes.Role, roleName)//會員角色
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

                // 1. 驗證帳密成功
                string memberId = user.MemberID;

            // 2. 同步 Session → DB
            _cartService.SyncCartAfterLogin(memberId);

            // 3. DB → Session（確保乾淨）
            _cartService.ClearSessionCart();

            // 4. 導回原頁
            if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _cartService.ClearSessionCart();
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
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(VMForgetPassword vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = _context.Privacies.FirstOrDefault(u => u.Email == vm.Email);
            if (user == null)
            {
                // 安全考量，不暴露Email是否存在
                TempData["Message"] = "已發送重設密碼 Email，請檢查收件匣";
                return RedirectToAction("Login");
            }

            // 產生 Token
            var token = Guid.NewGuid().ToString("N") + "-" + Random.Shared.Next(1000, 9999);
            var expiry = DateTime.Now.AddHours(1);

            var resetToken = new PasswordResetToken
            {
                MemberID = user.MemberID,
                Token = token,
                ExpiryDate = expiry,
                Used = false
            };
            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // 寄送 Email (用你的 Email Service)
            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);
            // SendEmail(vm.Email, "重設密碼", $"請點擊此連結重設密碼：{resetLink}");

            TempData["Message"] = "已發送重設密碼 Email，請檢查收件匣";
            TempData["ResetLink"] = resetLink;//測試用，實際不應顯示在畫面上
            return RedirectToAction("ForgetPasswordConfirmation");
        }
        //確認頁面
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        //重設密碼
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login");

            var vm = new VMResetPassword { Token = token };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(VMResetPassword vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var resetToken = _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == vm.Token && !t.Used && t.ExpiryDate > DateTime.Now);

            if (resetToken == null)
            {
                ModelState.AddModelError("", "此連結無效或已過期");
                return View(vm);
            }

            var user = _context.Privacies.FirstOrDefault(u => u.MemberID == resetToken.MemberID);
            if (user == null)
            {
                ModelState.AddModelError("", "使用者不存在");
                return View(vm);
            }

            // Hash 新密碼
            var hasher = new PasswordHasher<Privacy>();
            user.PasswordHash = hasher.HashPassword(user, vm.NewPassword);

            // 標記 Token 已使用
            resetToken.Used = true;

            await _context.SaveChangesAsync();

            TempData["Message"] = "密碼已重設成功，請重新登入";
            return RedirectToAction("Login");
        }
    }
}
