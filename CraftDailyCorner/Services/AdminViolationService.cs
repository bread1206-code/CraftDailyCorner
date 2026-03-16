using CraftDailyCorner.Areas.Admin.ViewModels.Violation;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Services
{
    public class AdminViolationService : IAdminViolationService
    {
        private readonly CraftDailyCornerContext _context;

        // ReportStatus
        private const byte REPORT_PENDING = 1;
        private const byte REPORT_VIOLATION = 2;
        private const byte REPORT_NORMAL = 3;

        // 你指定的「違規後要更新目標狀態」
        private const byte CREATOR_POST_VIOLATION_STATUS_ID = 2;
        private const byte CREATOR_PROFILE_VIOLATION_STATUS_ID = 2;
        private const byte PRODUCT_SUSPENDED_STATUS_ID = 4;

        public AdminViolationService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminViolationIndex> GetIndexAsync(string mode, string? memberId = null, int page = 1)
        {
            mode = (mode ?? "pending").Trim().ToLowerInvariant();

            const int pageSize = 8;
            page = page <= 0 ? 1 : page;

            var baseQuery = _context.Reports
                .AsNoTracking()
                .Include(r => r.ReportStatus)
                .Include(r => r.Reporter) // 檢舉人 (Report.MemberID)
                .AsQueryable();

            if (mode == "history")
            {
                // 歷史資料：Status != Pending
                baseQuery = baseQuery.Where(r => r.StatusID != REPORT_PENDING);

                // 右上角 MemberID 搜尋：命中「檢舉人」或「被檢舉者」
                if (!string.IsNullOrWhiteSpace(memberId))
                {
                    var keyword = memberId.Trim();

                    baseQuery = baseQuery.Where(r =>
                        r.MemberID == keyword ||

                        (r.ReportType == ReportTargetType.Post &&
                            _context.CreatorPosts.Any(p => p.PostID == r.TargetID && p.CreatorID == keyword)) ||

                        (r.ReportType == ReportTargetType.Product &&
                            _context.Products.Any(p => p.ProductID == r.TargetID && p.CreatorID == keyword)) ||

                        (r.ReportType == ReportTargetType.Portfolio &&
                            _context.Portfolios.Any(p => p.PortfolioID == r.TargetID && p.CreatorID == keyword)) ||

                        (r.ReportType == ReportTargetType.Comment &&
                            _context.PostComments.Any(c => c.CommentID == r.TargetID && c.MemberID == keyword))
                    );
                }
            }
            else
            {
                // 待處理：Pending
                baseQuery = baseQuery.Where(r => r.StatusID == REPORT_PENDING);
            }

            // 最舊優先
            baseQuery = baseQuery.OrderBy(r => r.CreatedAt);

            var total = await baseQuery.CountAsync();

            var rows = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new
                {
                    r.ReportID,
                    r.StatusID,
                    StatusName = r.ReportStatus!.StatusName,
                    r.CreatedAt,

                    ReporterMemberId = r.MemberID,
                    ReporterName = r.Reporter.DisplayName,

                    TargetType = r.ReportType,
                    r.TargetID,
                    Reason = r.ReasonCode
                })
                .ToListAsync();

            var items = new List<VMAdminViolationListItem>();

            foreach (var r in rows)
            {
                var (ownerId, ownerName) = await GetTargetOwnerAsync(r.TargetType, r.TargetID);

                items.Add(new VMAdminViolationListItem
                {
                    ReportID = r.ReportID,
                    StatusID = r.StatusID,
                    StatusName = r.StatusName,
                    CreatedAt = r.CreatedAt,

                    ReporterMemberID = r.ReporterMemberId,
                    ReporterName = r.ReporterName,

                    TargetOwnerID = ownerId,
                    TargetOwnerName = ownerName,

                    TargetType = (byte)r.TargetType,
                    TargetTypeName = GetEnumDisplayName(r.TargetType),
                    TargetID = r.TargetID,

                    Reason = (int)r.Reason,
                    ReasonName = GetEnumDisplayName(r.Reason)
                });
            }

            return new VMAdminViolationIndex
            {
                Mode = mode == "history" ? "history" : "pending",
                SearchMemberId = memberId,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<VMAdminViolationDetail?> GetDetailAsync(long reportId)
        {
            long id = reportId;

            var report = await _context.Reports
                .AsNoTracking()
                .Include(r => r.ReportStatus)
                .Include(r => r.Reporter).ThenInclude(m => m.Privacy) 
                .FirstOrDefaultAsync(r => r.ReportID == id);

            if (report == null) return null;

            var (ownerId, ownerName) = await GetTargetOwnerAsync(report.ReportType, report.TargetID);

            return new VMAdminViolationDetail
            {
                ReportID = (int)report.ReportID,
                StatusID = report.StatusID,
                StatusName = report.ReportStatus!.StatusName,
                CreatedAt = report.CreatedAt,

                ReporterMemberID = report.MemberID,
                ReporterName = report.Reporter.DisplayName,
                ReporterEmail = report.Reporter.Privacy?.Email,
                ReporterPhone = report.Reporter.Privacy?.Phone,

                TargetType = (byte)report.ReportType,
                TargetTypeName = GetEnumDisplayName(report.ReportType),
                TargetID = report.TargetID,
                UserReasonText=report.Reason,

                TargetOwnerID = ownerId,
                TargetOwnerName = ownerName,

                Reason = (int)report.ReasonCode,
                ReasonName = GetEnumDisplayName(report.ReasonCode),

                AdminNote = report.AdminNote,
                TargetUrl = await BuildTargetUrlAsync((ReportTargetType)report.ReportType, report.TargetID),
            };
        }

        public async Task MarkViolationAsync(long reportId, string adminMemberId, string? adminNote)
        {
            adminNote = (adminNote ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(adminNote))
                throw new ValidationException("判定違規時，管理者備註為必填。");

            if (adminNote.Length > 200)
                throw new ValidationException("管理者備註不可超過 200 字。");

            using var tx = await _context.Database.BeginTransactionAsync();

            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportID == reportId);

            if (report == null)
                throw new Exception("找不到該檢舉事件。");

            if (report.MemberID == adminMemberId)
                throw new ValidationException("禁止審核自己提交的檢舉。");

            if (report.StatusID != REPORT_PENDING)
                throw new Exception("此檢舉已完成審核，無法再次操作。");

            // 取得被檢舉目標的擁有者 MemberID
            var violatorMemberId = await GetTargetOwnerMemberIdAsync(
                    (ReportTargetType)report.ReportType,
                    report.TargetID);

            report.StatusID = REPORT_VIOLATION;
            report.AdminNote = adminNote;
            report.ReviewedAt = DateTime.Now;
            report.ReviewedBy = adminMemberId;
            
            await ApplyTargetViolationAsync((ReportTargetType)report.ReportType, report.TargetID);

            // 違規次數 +1
            if (!string.IsNullOrWhiteSpace(violatorMemberId))
            {
                var violator = await _context.Members
                    .FirstOrDefaultAsync(m => m.MemberID == violatorMemberId);

                if (violator == null)
                    throw new Exception("找不到被判定違規的會員資料。");

                violator.ViolationCount += 1;
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task MarkNormalAsync(long reportId, string adminMemberId, string? adminNote, bool isMalicious)
        {
            adminNote = (adminNote ?? string.Empty).Trim();

            if (adminNote.Length > 200)
                throw new ValidationException("管理者備註不可超過 200 字。");

            using var tx = await _context.Database.BeginTransactionAsync();

            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportID == reportId);

            if (report == null)
                throw new Exception("找不到該檢舉事件。");

            if (report.MemberID == adminMemberId)
                throw new ValidationException("禁止審核自己提交的檢舉。");

            if (report.StatusID != REPORT_PENDING)
                throw new Exception("此檢舉已完成審核，無法再次操作。");

            report.StatusID = REPORT_NORMAL;
            report.AdminNote = adminNote;
            report.ReviewedAt = DateTime.Now;
            report.ReviewedBy = adminMemberId;

            if (isMalicious)
            {
                var reporter = await _context.Members
                    .FirstOrDefaultAsync(m => m.MemberID == report.MemberID);

                if (reporter == null)
                    throw new Exception("找不到檢舉會員資料。");

                reporter.MaliciousReportCount += 1;

                if (reporter.MaliciousReportCount > 0 &&
                    reporter.MaliciousReportCount % 5 == 0)
                {
                    reporter.ReportBanUntil = DateTime.Now.AddHours(24);
                }
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task<long?> GetNextPendingIdAsync(long currentReportId, string adminMemberId)
        {
            var cur = await _context.Reports
        .AsNoTracking()
        .Where(r => r.ReportID == currentReportId)
        .Select(r => new { r.ReportID, r.CreatedAt })
        .FirstOrDefaultAsync();

            if (cur == null) return null;

            var next = await _context.Reports
                .AsNoTracking()
                .Where(r => r.StatusID == REPORT_PENDING)
                .Where(r => r.MemberID != adminMemberId)
                .Where(r =>
                    r.CreatedAt > cur.CreatedAt ||
                    (r.CreatedAt == cur.CreatedAt && r.ReportID > cur.ReportID)
                )
                .OrderBy(r => r.CreatedAt)
                .ThenBy(r => r.ReportID)
                .Select(r => (long?)r.ReportID)
                .FirstOrDefaultAsync();

            return next;
        }

        // =============================
        // private helpers
        // =============================

        private async Task ApplyTargetViolationAsync(ReportTargetType type, string targetId)
        {
            switch (type)
            {
                case ReportTargetType.Post:
                    {
                        var post = await _context.CreatorPosts.FirstOrDefaultAsync(x => x.PostID == targetId);
                        if (post == null) throw new Exception("找不到被檢舉的日誌。");

                        post.StatusID = CREATOR_POST_VIOLATION_STATUS_ID;
                        return;
                    }
                case ReportTargetType.Product:
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == targetId);
                        if (product == null) throw new Exception("找不到被檢舉的商品。");

                        product.StatusID = PRODUCT_SUSPENDED_STATUS_ID;
                        return;
                    }
                case ReportTargetType.Portfolio:
                    {
                        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(x => x.PortfolioID == targetId);
                        if (portfolio == null) throw new Exception("找不到被檢舉的作品集。");

                        var profile = await _context.CreatorProfiles.FirstOrDefaultAsync(x => x.CreatorID == portfolio.CreatorID);
                        if (profile == null) throw new Exception("找不到該作品集對應的創作者資料。");

                        profile.StatusID = CREATOR_PROFILE_VIOLATION_STATUS_ID;
                        return;
                    }
                case ReportTargetType.Comment:
                    {
                        var comment = await _context.PostComments.FirstOrDefaultAsync(x => x.CommentID == targetId);
                        if (comment == null) throw new Exception("找不到被檢舉的留言。");

                        comment.Status = (PostCommentStatus)1; // Violation
                        return;
                    }
                default:
                    throw new Exception("未知的檢舉類型。");
            }
        }

        private async Task<(string? OwnerId, string? OwnerName)> GetTargetOwnerAsync(ReportTargetType type, string targetId)
        {
            switch (type)
            {
                case ReportTargetType.Comment:
                    {
                        var comment = await _context.PostComments
                            .AsNoTracking()
                            .Include(c => c.Member)
                            .FirstOrDefaultAsync(c => c.CommentID == targetId);

                        return comment == null ? (null, null) : (comment.MemberID, comment.Member?.DisplayName);
                    }
                case ReportTargetType.Post:
                    {
                        var post = await _context.CreatorPosts
                            .AsNoTracking()
                            .Include(p => p.CreatorProfile).ThenInclude(cp => cp.Member)
                            .FirstOrDefaultAsync(p => p.PostID == targetId);

                        if (post == null) return (null, null);

                        return (post.CreatorID, post.CreatorProfile?.Member?.DisplayName);
                    }
                case ReportTargetType.Product:
                    {
                        var product = await _context.Products
                            .AsNoTracking()
                            .Include(p => p.CreatorProfile).ThenInclude(cp => cp.Member)
                            .FirstOrDefaultAsync(p => p.ProductID == targetId);

                        if (product == null) return (null, null);

                        return (product.CreatorID, product.CreatorProfile?.Member?.DisplayName);
                    }
                case ReportTargetType.Portfolio:
                    {
                        var portfolio = await _context.Portfolios
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.PortfolioID == targetId);

                        if (portfolio == null) return (null, null);

                        var profile = await _context.CreatorProfiles
                            .AsNoTracking()
                            .Include(cp => cp.Member)
                            .FirstOrDefaultAsync(cp => cp.CreatorID == portfolio.CreatorID);

                        return profile == null
                            ? (portfolio.CreatorID, null)
                            : (profile.CreatorID, profile.Member?.DisplayName);
                    }
                default:
                    return (null, null);
            }
        }

        private static string GetEnumDisplayName<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            var member = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            var display = member?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            return display?.Name ?? value.ToString();
        }

        private async Task<string?> BuildTargetUrlAsync(ReportTargetType type, string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId))
                return null;

            switch (type)
            {
                case ReportTargetType.Product:
                    return $"/Products/Detail/{targetId}";

                case ReportTargetType.Post:
                    return $"/Post/Detail/{targetId}";

                case ReportTargetType.Portfolio:

                    return $"/Portfolio/Detail/{targetId}";

                case ReportTargetType.Comment:
                    // 最標準：跳到該留言所屬日誌，並用錨點定位到留言
                    var postId = await _context.PostComments
                        .AsNoTracking()
                        .Where(c => c.CommentID == targetId)
                        .Select(c => c.PostID)
                        .FirstOrDefaultAsync();

                    return string.IsNullOrWhiteSpace(postId)
                        ? null
                        : $"/Post/Detail/{postId}#comment-{targetId}";

                default:
                    return null;
            }
        }
        //被檢舉目標的擁有者 MemberID
        private async Task<string?> GetTargetOwnerMemberIdAsync(ReportTargetType type, string targetId)
        {
            switch (type)
            {
                case ReportTargetType.Comment:
                    {
                        return await _context.PostComments
                            .AsNoTracking()
                            .Where(c => c.CommentID == targetId)
                            .Select(c => c.MemberID)
                            .FirstOrDefaultAsync();
                    }

                case ReportTargetType.Post:
                    {
                        return await _context.CreatorPosts
                            .AsNoTracking()
                            .Where(p => p.PostID == targetId)
                            .Select(p => p.CreatorProfile.MemberID)
                            .FirstOrDefaultAsync();
                    }

                case ReportTargetType.Product:
                    {
                        return await _context.Products
                            .AsNoTracking()
                            .Where(p => p.ProductID == targetId)
                            .Select(p => p.CreatorProfile.MemberID)
                            .FirstOrDefaultAsync();
                    }

                case ReportTargetType.Portfolio:
                    {
                        return await _context.Portfolios
                            .AsNoTracking()
                            .Where(p => p.PortfolioID == targetId)
                            .Select(p => p.CreatorProfile.MemberID)
                            .FirstOrDefaultAsync();
                    }

                default:
                    return null;
            }
        }
    }
}