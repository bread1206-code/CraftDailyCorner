using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MemberSecurityService : IMemberSecurityService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IPasswordHasher<Privacy> _passwordHasher;

        public MemberSecurityService(
            CraftDailyCornerContext context,
            IPasswordHasher<Privacy> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<(bool ok, string message)> ChangePasswordAsync(
            string memberId,
            string currentPassword,
            string newPassword)
        {
            var privacy = await _context.Privacies
                .FirstOrDefaultAsync(p => p.MemberID == memberId);

            if (privacy == null)
                return (false, "找不到會員隱私資料");

            var verify = _passwordHasher.VerifyHashedPassword(
                privacy,
                privacy.PasswordHash,
                currentPassword);

            if (verify == PasswordVerificationResult.Failed)
                return (false, "目前密碼錯誤");

            if (currentPassword == newPassword)
                return (false, "新密碼不可與舊密碼相同");

            privacy.PasswordHash =
                _passwordHasher.HashPassword(privacy, newPassword);

            await _context.SaveChangesAsync();

            return (true, "密碼修改成功");
        }
    }
}