using CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminCreatorReviewService : IAdminCreatorReviewService
    {
        private readonly CraftDailyCornerContext _context;

        public AdminCreatorReviewService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminCreatorReviewIndex> GetIndexAsync()
        {
            var apps = await _context.CreatorApplications
                .AsNoTracking()
                .Include(x => x.Member)
                .Include(x => x.CreatorApplicationStatus)
                .OrderBy(x => x.StatusID)             // 待審核排最前
                .ThenByDescending(x => x.AppliedAt)   // 同狀態最新在前
                .Select(x => new VMAdminCreatorReviewListItem
                {
                    ApplicationID = x.ApplicationID,
                    MemberID = x.MemberID,
                    MemberName = x.Member.DisplayName,
                    Email = x.Member.Privacy.Email,
                    Phone = x.Member.Privacy.Phone,
                    DisplayName = x.DisplayName,
                    AppliedAt = x.AppliedAt,
                    StatusID = x.StatusID,
                    StatusName = x.CreatorApplicationStatus.StatusName
                })
                .ToListAsync();

            return new VMAdminCreatorReviewIndex
            {
                Items = apps
            };
        }

        public async Task<VMAdminCreatorReviewDetail?> GetDetailAsync(int applicationId)
        {
            var app = await _context.CreatorApplications
                .AsNoTracking()
                .Include(x => x.Member)
                .Include(x => x.CreatorApplicationStatus)
                .Include(x => x.Reviewer)
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null) return null;

            return new VMAdminCreatorReviewDetail
            {
                ApplicationID = app.ApplicationID,
                MemberID = app.MemberID,
                MemberName = app.Member?.DisplayName ?? "(未知會員)",
                BrandName = app.DisplayName,
                BrandIntro = app.Intro,
                StartDate = app.StartDate,
                PortfolioUrl = app.PortfolioSampleUrl,

                StatusID = app.StatusID,
                StatusName = app.CreatorApplicationStatus?.StatusName ?? "(未知狀態)",

                ReviewedAt = app.ReviewedAt,
                ReviewerName = app.Reviewer?.DisplayName,     // ✅ 允許 null
                ReviewNote = app.ReviewNote              // ✅ 允許 null
            };
        }

        public async Task ApproveAsync(int applicationId, string adminMemberId, string? reviewNote)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var app = await _context.CreatorApplications
                .Include(x => x.Member)
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null)
                throw new Exception("找不到申請資料");

            if (app.StatusID != 1)
                throw new Exception("此申請已審核，無法重複操作");

            // 產生 CreatorID：C00001, C00002...
            var newCreatorId = await GenerateNextCreatorIdAsync();

            // 建立 CreatorProfile（你的 Seed 也有 CreatorProfile 概念）
            _context.CreatorProfiles.Add(new CreatorProfile
            {
                CreatorID = newCreatorId,
                MemberID = app.MemberID,
                DisplayName = app.DisplayName,
                Intro = app.Intro ?? string.Empty,
                BankCode = ("" ?? string.Empty).Trim(),
                BankAccount = ("" ?? string.Empty).Trim(),
                StatusID = 1, //1 = 啟用
                ImageUrl = "default", // 沒上傳品牌圖就用預設（你前台也有 default.png 的慣例）
                CreatedAt = DateTime.Now
            });

            // 掛上 Creator 角色（02）
            var hasRole = await _context.MemberRoles
                .AnyAsync(r => r.MemberID == app.MemberID && r.RoleID == "02");

            if (!hasRole)
            {
                _context.MemberRoles.Add(new MemberRole
                {
                    MemberID = app.MemberID,
                    RoleID = "02",
                    AssignedAt = DateTime.Now
                });

                _context.MemberRoleHistories.Add(new MemberRoleHistory
                {
                    Action = (MemberRoleHistoryAction)1,          // 1 = Add（依你 Seed 的寫法）
                    OperatedAt = DateTime.Now,
                    MemberID = app.MemberID,
                    RoleID = "02",
                    OperatedBy = (MemberRoleHistoryOperated)1,    // 1 = Admin（依你 Seed 的寫法）
                    OperatorMemberID = adminMemberId
                });
            }

            // 更新申請狀態
            app.StatusID = 2; // 2 = 通過（依你 Seed：另有 3 = 未通過）
            app.ReviewedAt = DateTime.Now;
            app.ReviewedBy = adminMemberId;
            app.ReviewNote = (reviewNote ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task RejectAsync(int applicationId, string adminMemberId, string reviewNote)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null)
                throw new Exception("找不到申請資料");

            if (app.StatusID != 1)
                throw new Exception("此申請已審核，無法重複操作");

            if (string.IsNullOrWhiteSpace(reviewNote))
                throw new Exception("請填寫未通過原因");

            app.StatusID = 3; // 3 = 未通過
            app.ReviewedAt = DateTime.Now;
            app.ReviewedBy = adminMemberId;
            app.ReviewNote = reviewNote.Trim();

            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateNextCreatorIdAsync()
        {
            // 取最大 CreatorID（格式：C00001）
            var lastId = await _context.CreatorProfiles
                .AsNoTracking()
                .OrderByDescending(c => c.CreatorID)
                .Select(c => c.CreatorID)
                .FirstOrDefaultAsync();

            var nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastId) && lastId.Length == 6 && lastId.StartsWith("C"))
            {
                if (int.TryParse(lastId.Substring(1), out var n))
                    nextNumber = n + 1;
            }

            return "C" + nextNumber.ToString("D5");
        }
    }
}