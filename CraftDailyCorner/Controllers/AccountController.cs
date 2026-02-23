using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMLogin login, string? returnUrl = null)
        {
            var user = _context.Privacies
                .FirstOrDefault(u => u.Email == login.Account || u.Phone == login.Account);

            if (user == null)
                return Json(new { success = false, message = "帳號或密碼錯誤" });

            var hasher = new PasswordHasher<Privacy>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (result == PasswordVerificationResult.Failed)
                return Json(new { success = false, message = "帳號或密碼錯誤" });

            
            await SignInMemberAsync(user.MemberID);

            // RememberAccount 邏輯保留
            if (login.RememberAccount)
            {
                Response.Cookies.Append(
                    "RememberAccount",
                    login.Account,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(30),
                        HttpOnly = true
                    });
            }
            else
            {
                Response.Cookies.Delete("RememberAccount");
            }

            return Json(new { success = true });
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VMRegister model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Any())
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value!.Errors.First().ErrorMessage
                    );

                return Json(new { success = false, errors });
            }

            if (_context.Privacies.Any(p => p.Email == model.Email))
            {
                return Json(new
                {
                    success = false,
                    errors = new { Email = "此 Email 已被註冊" }
                });
            }

            if (_context.Privacies.Any(p => p.Phone == model.Phone))
            {
                return Json(new
                {
                    success = false,
                    errors = new { Phone = "此手機號碼已被註冊" }
                });
            }

            // 建立會員
            string newMemberId = await _memberService.RegisterMemberAsync(model);

            // 自動登入
            await SignInMemberAsync(newMemberId);

            return Json(new { success = true });
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

            // 寄送 Email 
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
        private async Task SignInMemberAsync(string memberId)
        {
            // 1️ 取得顯示名稱
            var displayName = _context.Members
                .Where(m => m.MemberID == memberId)
                .Select(m => m.DisplayName)
                .FirstOrDefault() ?? "使用者";

            // 2️ 取得角色

            var roles = (
                from mr in _context.MemberRoles.AsNoTracking()
                join r in _context.Roles.AsNoTracking()
                    on mr.RoleID equals r.RoleID
                where mr.MemberID == memberId
                select r.RoleID
            ).Distinct().ToList();//RoleID "01"一般會員     "02"創作者     "03"管理者

            if (!roles.Any())
            {
                roles.Add("01"); // 預設為一般會員
            }

            // 3️ 建立 Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, memberId),
                new Claim(ClaimTypes.Name, displayName)
                
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // 4如果是創作者，加入 CreatorID Claim
            if (roles.Contains("02"))
            {
                var creatorId = _context.CreatorProfiles
                    .Where(c => c.MemberID == memberId)
                    .Select(c => c.CreatorID)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(creatorId))
                {
                    claims.Add(new Claim("CreatorID", creatorId));
                }
            }
            // 5 登入
            var identity = new ClaimsIdentity(claims, "CraftDailyCornerLogin");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CraftDailyCornerLogin", principal);
        }

    }
}
