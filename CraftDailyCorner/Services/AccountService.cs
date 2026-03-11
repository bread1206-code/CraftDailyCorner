using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Services
{
    public class AccountService : IAccountService
    {
        private readonly CraftDailyCornerContext _context;

        public AccountService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterMemberAsync(VMRegister vm)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                // 1. 呼叫 SP 生成 MemberID
                var newMemberIdParam = new SqlParameter
                {
                    ParameterName = "@NewMemberID",
                    SqlDbType = System.Data.SqlDbType.Char,
                    Size = 8,
                    Direction = System.Data.ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC getCreatedMemberID @DisplayName, @NewMemberID OUTPUT",
                    new SqlParameter("@DisplayName", vm.DisplayName),
                    newMemberIdParam
                );

                string newMemberId = (string)newMemberIdParam.Value;

                // 2. 建立 Member
                var member = new Member
                {
                    MemberID = newMemberId,
                    ImageUrl = null,
                    DisplayName = vm.DisplayName,
                    StatusID = 1,
                    CreatedAt = now
                };
                _context.Members.Add(member);

                // 3. 建立 Privacy 並 Hash 密碼
                var hasher = new PasswordHasher<Privacy>();

                var privacy = new Privacy
                {
                    MemberID = newMemberId,
                    Email = vm.Email,
                    Phone = vm.Phone,
                    Gender = vm.Gender!.Value,
                    PasswordHash = hasher.HashPassword(null!, vm.Password)
                };
                _context.Privacies.Add(privacy);

                // 4. 指派預設角色 (RoleID = 01)
                var memberRole = new MemberRole
                {
                    MemberID = newMemberId,
                    RoleID = "01",
                    AssignedAt = now
                };
                _context.MemberRoles.Add(memberRole);

                // 5. 紀錄角色指派歷史
                var memberRoleHistory = new MemberRoleHistory
                {
                    Action = MemberRoleHistoryAction.Created,
                    OperatedAt = now,
                    MemberID = newMemberId,
                    RoleID = "01",
                    OperatedBy = MemberRoleHistoryOperated.System,
                    OperatorMemberID = null
                };
                _context.MemberRoleHistories.Add(memberRoleHistory);

                // 6. 建立會員購物車
                var cart = new Cart
                {
                    MemberID = newMemberId,
                    CreatedAt = now
                };
                _context.Carts.Add(cart);

                // 7. 建立通知偏好設定（預設全部開啟）
                var defaultTypes = new List<NotificationType>
                {
                    NotificationType.Announcement,

                    NotificationType.FavoriteProductPublished,
                    NotificationType.FavoriteProductRestocked,

                    NotificationType.CreatorNewPost,
                    NotificationType.CreatorNewProduct,
                    NotificationType.CreatorNewPortfolio,

                    NotificationType.OrderCreated,
                    NotificationType.OrderPaid,
                    NotificationType.OrderShipped,
                    NotificationType.OrderDelivered,
                    NotificationType.OrderCompleted,

                    NotificationType.ProductLowStock,
                    NotificationType.ProductOutOfStock,
                    NotificationType.PostComment
                };

                var notificationPreferences = defaultTypes
                    .Select(type => new NotificationPreference
                    {
                        MemberID = newMemberId,
                        NotificationType = type,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now
                    })
                    .ToList();

                _context.NotificationPreferences.AddRange(notificationPreferences);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return newMemberId;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

    }
}
