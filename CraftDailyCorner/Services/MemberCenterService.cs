using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
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
            var member = _context.Members
                .Include(m => m.Privacy)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                throw new Exception("會員不存在");

            return new VMMemberDashboard
            {
                DisplayName = member.DisplayName,
                Email = member.Privacy?.Email ?? string.Empty,
                CreatedAt = member.CreatedAt,

                OrderCount = _context.Orders
                    .Count(o => o.MemberID == memberId),

                PendingPaymentCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.OrderStatus.StatusID == 1),//待付款

                CompletedOrderCount = _context.Orders
                    .Count(o =>
                        o.MemberID == memberId &&
                        o.OrderStatus.StatusID == 4)//已完成
            };
        }
        public VMEditProfile GetProfile(string memberId)
        {
            var member = _context.Members
                .Include(m => m.Privacy)
                .FirstOrDefault(m => m.MemberID == memberId);

            if (member == null)
                throw new Exception("會員不存在");

            return new VMEditProfile
            {
                MemberID = member.MemberID,
                DisplayName = member.DisplayName,
                Email = member.Privacy?.Email ?? string.Empty,
                Phone = member.Privacy?.Phone
            };
        }

        public void UpdateProfile(VMEditProfile vm)
        {
            var member = _context.Members
                .Include(m => m.Privacy)
                .FirstOrDefault(m => m.MemberID == vm.MemberID);

            if (member == null)
                throw new Exception("會員不存在");

            member.DisplayName = vm.DisplayName;

            if (member.Privacy != null)
            {
                member.Privacy.Email = vm.Email;
                member.Privacy.Phone = vm.Phone;
            }

            _context.SaveChanges();
        }

    }
}