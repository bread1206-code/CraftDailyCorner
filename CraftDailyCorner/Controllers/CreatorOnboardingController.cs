using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class CreatorOnboardingController : Controller
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorOnboardingController(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 會員：審核通過但尚未成為創作者 → 進確認頁
        public async Task<IActionResult> ApprovedConfirm()
        {
            var memberId = User.GetMemberId();

            // 已是創作者就不需要走這段
            var isCreator = await _context.MemberRoles
                .AnyAsync(x => x.MemberID == memberId && x.RoleID == "02");

            if (isCreator)
                return RedirectToAction("Dashboard", "Creator");

            // 找最新一筆申請（Approved）
            var app = await _context.CreatorApplications
                .AsNoTracking()
                .Where(x => x.MemberID == memberId && x.StatusID == 2)
                .OrderByDescending(x => x.AppliedAt)
                .FirstOrDefaultAsync();

            if (app == null)
                return RedirectToAction("Index", "Member"); // 沒有 Approved 申請就回會員中心

            // 直接把申請資料帶到 view 讓他確認（品牌名/簡介/作品照/起始日）
            return View(app);
        }

        // 會員按確認 → 這裡才發 CreatorID + 建 CreatorProfile + 掛角色
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovedConfirmSubmit(int applicationId)
        {
            var memberId = User.GetMemberId();

            using var tx = await _context.Database.BeginTransactionAsync();

            // 重新鎖住申請資料，避免重複送出
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId && x.MemberID == memberId);

            if (app == null)
                return NotFound();

            if (app.StatusID != 2)
                throw new Exception("申請狀態不允許確認（需為已通過）。");

            // 若已是創作者，避免重複建立
            var alreadyCreator = await _context.MemberRoles
                .AnyAsync(x => x.MemberID == memberId && x.RoleID == "02");

            if (alreadyCreator)
            {
                await tx.RollbackAsync();
                return RedirectToAction("Dashboard", "Creator");
            }

            //  呼叫 SP 取得 CreatorID（只在此時發號）
            var newCreatorIdParam = new SqlParameter
            {
                ParameterName = "@NewCreatorID",
                SqlDbType = SqlDbType.Char,
                Size = 6,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC getCreatedCreatorID @NewCreatorID OUTPUT",
                newCreatorIdParam
            );

            var newCreatorId = (string)newCreatorIdParam.Value;

            //  建立 CreatorProfile（狀態：啟用）
            _context.CreatorProfiles.Add(new CreatorProfile
            {
                CreatorID = newCreatorId,
                MemberID = memberId,

                BrandName = (app.BrandName ?? app.BrandName ?? string.Empty).Trim(),
                BrandIntro = (app.BrandIntro ?? string.Empty).Trim(),
                StartDate = app.StartDate,

                StatusID = 1,
                ImageUrl = "default",
                CreatedAt = DateTime.Now
            });

            //  掛 Creator 角色 + 寫歷史
            _context.MemberRoles.Add(new MemberRole
            {
                MemberID = memberId,
                RoleID = "02",
                AssignedAt = DateTime.Now
            });

            _context.MemberRoleHistories.Add(new MemberRoleHistory
            {
                Action = (MemberRoleHistoryAction)1,        // Add
                OperatedAt = DateTime.Now,
                MemberID = memberId,
                RoleID = "02",
                OperatedBy = (MemberRoleHistoryOperated)1, 
                OperatorMemberID = ""//adminId
            });

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            TempData["Success"] = "創作者資料已建立！目前狀態為待啟用。";
            return RedirectToAction("Dashboard", "Creator");
        }
    }
}