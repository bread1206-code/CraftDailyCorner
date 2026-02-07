using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front.Member;
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
            // 取得會員基本資料（包含隱私設定）
            var member = GetMemberWithPrivacy(memberId);

            return new VMMemberDashboard
            {
                // 會員識別
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                CreatedAt = member.CreatedAt,

                // 訂單相關
                PendingPaymentCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.StatusID == 1), // 待付款

                OrderCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.StatusID != 1 &&
                        o.StatusID != 4), // 進行中

                AllOrderCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId), // 所有訂單（包含已完成、已取消）

                // 付款紀錄
                PaymentCount = _context.Payments
                    .Count(p => p.Order.MemberID == memberId),

                // 擴充功能（目前未實作）
                FavoriteCount = 0,
                FollowingCount = 0
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