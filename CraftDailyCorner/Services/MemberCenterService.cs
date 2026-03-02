using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MemberCenterService : IMemberCenterService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public MemberCenterService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        public VMMemberDashboard GetDashboard(string memberId)
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
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl ?? string.Empty,
                CreatedAt = member.CreatedAt,
                IsCreator = isCreator,
                CreatorApplicationStatusCode = applicationStatusCode,

                PendingPaymentCount = _context.Orders
                    .Count(o => o.MemberID == memberId && o.StatusID == 1),

                OrderCount = _context.Orders
                    .Count(o => o.MemberID == memberId && o.StatusID != 1 && o.StatusID != 5),

                AllOrderCount = _context.Orders
                    .Count(o => o.MemberID == memberId),

                PaymentCount = _context.Payments
                    .Count(p => p.Order.MemberID == memberId),

                FavoriteCount = _context.FavoriteProducts
                    .Count(fp => fp.MemberID == memberId),

                FollowingCount = _context.FollowCreators
                    .Count(f => f.MemberID == memberId)
            };
        }

        public VMEditProfile GetProfile(string memberId)
        {
            var member = GetMemberWithPrivacy(memberId);

            return new VMEditProfile
            {
                MemberID = member.MemberID,
                DisplayName = member.DisplayName,
                Email = member.Privacy?.Email ?? string.Empty,
                Phone = member.Privacy?.Phone ?? string.Empty,
                ImageUrl = member.ImageUrl ?? string.Empty,
            };
        }

        public void UpdateProfile(string memberId, VMEditProfile vm)
        {
            memberId = memberId.Trim();

            // 防止使用者改到別人的 MemberID（即使畫面上帶了 hidden）
            vm.MemberID = memberId;

            var member = GetMemberWithPrivacy(memberId);
            //手機重複檢查
            if (!string.IsNullOrWhiteSpace(vm.Phone))
            {
                var phoneExists = _context.Privacies
                    .Any(p => p.Phone == vm.Phone
                           && p.MemberID != memberId);

                if (phoneExists)
                {
                    throw new Exception("此手機號碼已被使用");
                }
            }
            member.DisplayName = vm.DisplayName;

            if (member.Privacy != null)
            {
                member.Privacy.Email = vm.Email;
                member.Privacy.Phone = vm.Phone;
            }

            // ===== 頭像上傳邏輯：從 Controller 搬進來 =====
            if (vm.AvatarFile != null && vm.AvatarFile.Length > 0)
            {
                var fileKey = string.IsNullOrEmpty(member.ImageUrl)
                    ? Guid.NewGuid().ToString()
                    : member.ImageUrl;

                _imageUploadService.UploadImage(
                    file: vm.AvatarFile,
                    seedSourcePath: null,
                    folderName: "01Member",
                    sizes: ImageSizePresets.Member,
                    entityId: fileKey
                );

                member.ImageUrl = fileKey;
                vm.ImageUrl = fileKey; // 如果你 View 會回顯用得到
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