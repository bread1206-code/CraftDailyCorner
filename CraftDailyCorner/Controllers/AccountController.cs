using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CraftDailyCorner.Controllers
{
    public class AccountController : Controller
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController(
            CraftDailyCornerContext context,
            IAccountService accountService,
            IAuthService authService)
        {
            _context = context;
            _accountService = accountService;
            _authService = authService;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            // 如果已經登入，直接導回首頁或 returnUrl
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

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

            // ✅ 改用 AuthService
            await _authService.SignInMemberAsync(HttpContext, user.MemberID);

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
            // ✅ 改用 AuthService
            await _authService.SignOutAsync(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        // =============================
        // Register
        // =============================
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        //註冊
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
                }, new JsonSerializerOptions { PropertyNamingPolicy = null });
            }

            if (_context.Privacies.Any(p => p.Phone == model.Phone))
            {
                return Json(new
                {
                    success = false,
                    errors = new { Phone = "此手機號碼已被註冊" }
                }, new JsonSerializerOptions { PropertyNamingPolicy = null });
            }

            // 建立會員
            string newMemberId = await _accountService.RegisterMemberAsync(model);

            // 自動登入（✅ 改用 AuthService）
            await _authService.SignInMemberAsync(HttpContext, newMemberId);

            return Json(new { success = true });
        }

        // =============================
        // Forget Password
        // =============================
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
                TempData["Message"] = "已發送重設密碼 Email，請檢查收件匣";
                return RedirectToAction("Login");
            }

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

            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);

            TempData["Message"] = "已發送重設密碼 Email，請檢查收件匣";
            TempData["ResetLink"] = resetLink; // 測試用
            return RedirectToAction("ForgetPasswordConfirmation");
        }

        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

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

            var hasher = new PasswordHasher<Privacy>();
            user.PasswordHash = hasher.HashPassword(user, vm.NewPassword);

            resetToken.Used = true;

            await _context.SaveChangesAsync();

            TempData["Message"] = "密碼已重設成功，請重新登入";
            return RedirectToAction("Login");
        }
    }
}