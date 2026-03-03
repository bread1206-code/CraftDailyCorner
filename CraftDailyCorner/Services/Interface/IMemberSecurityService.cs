namespace CraftDailyCorner.Services.Interface
{
    public interface IMemberSecurityService
    {
        /// <summary>
        /// 修改會員密碼（需驗證目前密碼）
        /// </summary>
        Task<(bool ok, string message)> ChangePasswordAsync(string memberId, string currentPassword, string newPassword);
    }
}