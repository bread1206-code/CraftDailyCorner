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

        public async Task<VMAdminCreatorReviewIndex> GetIndexAsync(string mode, string? memberId = null)
        {
            mode = (mode ?? "pending").Trim().ToLower();

            var query = _context.CreatorApplications
                .AsNoTracking()
                .Include(x => x.Member)
                    .ThenInclude(m => m.Privacy)
                .Include(x => x.CreatorApplicationStatus)
                .AsQueryable();

            if (mode == "history")
            {
                //  有輸入 MemberID：顯示該會員所有申請資料（含 pending / confirm 等）
                if (!string.IsNullOrWhiteSpace(memberId))
                {
                    memberId = memberId.Trim();

                    query = query
                        .Where(x => x.MemberID == memberId)
                        .OrderByDescending(x => x.AppliedAt);
                }
                else
                {
                    // ✅ 無輸入 MemberID：只顯示「已通過/已拒絕」
                    query = query
                        .Where(x => x.StatusID == 2 || x.StatusID == 3)
                        .OrderByDescending(x => x.ReviewedAt ?? x.AppliedAt);
                }
            }
            else
            {
                // ✅ 待審核頁：只顯示待審核
                query = query
                    .Where(x => x.StatusID == 1)
                    .OrderByDescending(x => x.AppliedAt);
            }

            var apps = await query
                .Select(x => new VMAdminCreatorReviewListItem
                {
                    ApplicationID = x.ApplicationID,
                    MemberID = x.MemberID,
                    MemberName = x.Member.DisplayName,
                    Email = x.Member.Privacy.Email,
                    Phone = x.Member.Privacy.Phone,
                    BrandName = x.BrandName,
                    AppliedAt = x.AppliedAt,
                    StatusID = x.StatusID,
                    StatusName = x.CreatorApplicationStatus.StatusName
                })
                .ToListAsync();

            return new VMAdminCreatorReviewIndex
            {
                Mode = mode,
                SearchMemberId = memberId,
                Items = apps
            };
        }

        public async Task<VMAdminCreatorReviewDetail?> GetDetailAsync(int applicationId)
        {
            var app = await _context.CreatorApplications
                .AsNoTracking()
                .Include(x => x.Member)
                    .ThenInclude(m => m.Privacy)
                .Include(x => x.CreatorApplicationStatus)
                .Include(x => x.Reviewer)
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null) return null;

            return new VMAdminCreatorReviewDetail
            {
                ApplicationID = app.ApplicationID,

                StatusID = app.StatusID,
                StatusName = app.CreatorApplicationStatus?.StatusName ?? "（未知狀態）",

                AppliedAt = app.AppliedAt,

                MemberID = app.MemberID,
                MemberName = app.Member?.DisplayName ?? "（未知會員）",
                Email = app.Member?.Privacy?.Email,
                Phone = app.Member?.Privacy?.Phone,

                BrandName = app.BrandName,
                BrandIntro = app.BrandIntro,
                PortfolioUrl = app.PortfolioSampleUrl,
                StartDate = app.StartDate,

                ReviewedAt = app.ReviewedAt,
                ReviewedBy = app.ReviewedBy,
                ReviewerName = app.Reviewer?.DisplayName,
                ReviewNote = app.ReviewNote
            };
        }

        // ===== Update: 保持現狀（不動） =====

        public async Task ApproveAsync(int applicationId, string adminMemberId, string? reviewNote)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null)
                throw new Exception("找不到申請資料");

            if (app.StatusID != 1)
                throw new Exception("此申請已審核，無法重複操作");

            if (app.MemberID == adminMemberId)
                throw new Exception("禁止審核自己的創作者申請");

            app.StatusID = 2; // Approved
            app.ReviewedAt = DateTime.Now;
            app.ReviewedBy = adminMemberId;
            app.ReviewNote = (reviewNote ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
        }

        public async Task RejectAsync(int applicationId, string adminMemberId, string reviewNote)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

            if (app == null)
                throw new Exception("找不到申請資料");

            if (app.StatusID != 1)
                throw new Exception("此申請已審核，無法重複操作");

            if (app.MemberID == adminMemberId)
                throw new Exception("禁止審核自己的創作者申請");

            if (string.IsNullOrWhiteSpace(reviewNote))
                throw new Exception("請填寫未通過原因");

            app.StatusID = 3; // Rejected
            app.ReviewedAt = DateTime.Now;
            app.ReviewedBy = adminMemberId;
            app.ReviewNote = reviewNote.Trim();

            await _context.SaveChangesAsync();
        }
        //  新增：下一筆待審核（同樣排除審核者自己的申請）
        public async Task<int?> GetNextPendingIdAsync(int currentApplicationId, string adminMemberId)
        {
            // 先抓目前這筆的排序基準
            var current = await _context.CreatorApplications
                .AsNoTracking()
                .Where(x => x.ApplicationID == currentApplicationId)
                .Select(x => new { x.ApplicationID, x.AppliedAt })
                .FirstOrDefaultAsync();

            if (current == null)
                return null;

            // 依「AppliedAt DESC, ApplicationID DESC」視為隊列順序
            // 找「下一筆」= 排序上比目前更後面的那一筆
            var nextId = await _context.CreatorApplications
                .AsNoTracking()
                .Where(x =>
                    x.StatusID == 1 &&
                    x.MemberID != adminMemberId &&               // 排除自己的申請
                    (x.AppliedAt < current.AppliedAt ||
                    (x.AppliedAt == current.AppliedAt && x.ApplicationID < current.ApplicationID)))
                .OrderByDescending(x => x.AppliedAt)
                .ThenByDescending(x => x.ApplicationID)
                .Select(x => (int?)x.ApplicationID)
                .FirstOrDefaultAsync();

            return nextId;
        }
    }
}