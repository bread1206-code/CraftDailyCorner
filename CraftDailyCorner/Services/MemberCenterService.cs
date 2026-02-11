using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MemberCenterService
    {
        private readonly CraftDailyCornerContext _context;

        public MemberCenterService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public VMMemberDashboard GetMemberDashboard(string memberId)
        {
            memberId = memberId.Trim();

            var member = GetMemberWithPrivacy(memberId);

            // ===== 最新申請 =====
            var latestApplication = _context.CreatorApplications
                .Include(a => a.CreatorApplicationStatus)
                .Where(a => a.MemberID == memberId)
                .OrderByDescending(a => a.AppliedAt)
                .FirstOrDefault();

            string applicationStatusCode =
                latestApplication?.CreatorApplicationStatus?.StatusCode
                ?? "None";

            bool isCreator = _context.MemberRoles
                .Any(r => r.MemberID == memberId && r.RoleID == "02");

            return new VMMemberDashboard
            {
                // 會員識別
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl ?? string.Empty,
                CreatedAt = member.CreatedAt,
                IsCreator = isCreator,
                CreatorApplicationStatusCode = applicationStatusCode,

                // 訂單
                PendingPaymentCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.StatusID == 1),

                OrderCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.StatusID != 1 &&
                        o.StatusID != 4),

                AllOrderCount = _context.Orders
                    .Count(o => o.MemberID == memberId),

                // 付款
                PaymentCount = _context.Payments
                    .Count(p => p.Order.MemberID == memberId),

                // 收藏
                FavoriteCount = _context.FavoriteProducts
                    .Count(fp => fp.MemberID == memberId),

                // 追蹤
                FollowingCount = _context.FollowCreators
                    .Count(f => f.MemberID == memberId)
            };
        }
        public VMEditProfile GetProfile(string memberId)
        {
            // 取得會員基本資料（包含隱私設定）
            var member = GetMemberWithPrivacy(memberId);

            return new VMEditProfile
            {
                MemberID = member.MemberID,
                DisplayName = member.DisplayName,
                Email = member.Privacy?.Email ?? string.Empty,//若取得null則改用""(空字串)
                Phone = member.Privacy?.Phone ?? string.Empty,
                ImageUrl = member.ImageUrl ?? string.Empty,
            };
        }

        public void UpdateProfile(VMEditProfile vm)
        {
            // 取得會員基本資料（包含隱私設定）
            var member = GetMemberWithPrivacy(vm.MemberID);

            member.DisplayName = vm.DisplayName;

            if (member.Privacy != null)
            {
                member.Privacy.Email = vm.Email;
                member.Privacy.Phone = vm.Phone;
            }

            _context.SaveChanges();
        }
        private Member GetMemberWithPrivacy(string memberId)
        {
            var member = _context.Members
                .Include(m => m.Privacy)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                throw new Exception("會員不存在");

            return member;
        }
    }
    
}