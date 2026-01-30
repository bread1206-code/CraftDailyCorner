using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MemberService
    {
        private readonly CraftDailyCornerContext _context;

        public MemberService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterMemberAsync(VMRegister vm)
        {
            // 1. 呼叫 SP 生成 MemberID
            var newMemberIdParam = new SqlParameter
            {
                ParameterName = "@NewMemberID",
                SqlDbType = System.Data.SqlDbType.Char,
                Size = 8,
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC getCreateMember @DisplayName, @NewMemberID OUTPUT",
                new SqlParameter("@DisplayName", vm.DisplayName),
                newMemberIdParam
            );

            string newMemberId = (string)newMemberIdParam.Value;

            // 建立 Member
            var member = new Member
            {
                MemberID = newMemberId,
                ImageUrl = "default.png", // 預設頭像
                DisplayName = vm.DisplayName,
                Status = MemberStatus.Active,
                CreatedAt = DateTime.Now
            };
            _context.Members.Add(member);

            // 2. 建立 Privacy 並 Hash 密碼，設定預設頭像
            var hasher = new PasswordHasher<Privacy>();

            var privacy = new Privacy
            {
                MemberID = newMemberId,
                Email = vm.Email,
                Phone = vm.Phone,
                Gender = vm.Gender,
                PasswordHash = hasher.HashPassword(null!, vm.Password)
            };

            _context.Privacies.Add(privacy);

            // 3. 指派預設角色 (RoleID = 1)
            var memberRole = new MemberRole
            {
                MemberID = newMemberId,
                RoleID = "01",
                AssignedAt = DateTime.Now
            };
            _context.MemberRoles.Add(memberRole);
            // 4. 紀錄角色指派歷史
            var memberRoleHistory = new MemberRoleHistory
            {
                Action = MemberRoleHistoryAction.Created,
                OperatedAt = DateTime.Now,
                MemberID = newMemberId,
                RoleID = "01",
                OperatedBy = MemberRoleHistoryOperated.System,
                OperatorMemberID = null
            };
            _context.MemberRoleHistories.Add(memberRoleHistory);

            await _context.SaveChangesAsync();

            return newMemberId;
        }

    }
}
