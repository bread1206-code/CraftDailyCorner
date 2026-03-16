using CraftDailyCorner.Areas.Admin.ViewModels.Member;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminMemberService : IAdminMemberService
    {
        private readonly CraftDailyCornerContext _context;

        private const byte MEMBER_ACTIVE = 1;
        private const byte MEMBER_SUSPENDED = 2;

        public AdminMemberService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminMemberIndex> GetIndexAsync(string mode = "all", string? memberId = null, int page = 1)
        {
            mode = (mode ?? "all").Trim().ToLower();
            memberId = string.IsNullOrWhiteSpace(memberId) ? null : memberId.Trim();

            const int pageSize = 8;
            page = page <= 0 ? 1 : page;

            var query = _context.Members
                .AsNoTracking()
                .Include(x => x.MemberStatus)
                .Include(x => x.MemberRoles)
                    .ThenInclude(x => x.Role)
                .Include(x => x.CreatorProfile)
                .AsQueryable();

            switch (mode)
            {
                case "risk":
                    query = query
                        .Where(x => x.ViolationCount > 5)
                        .OrderByDescending(x => x.ViolationCount)
                        .ThenBy(x => x.MemberID);

                    var riskItems = await query
                        .Select(x => new VMAdminMemberListItem
                        {
                            MemberID = x.MemberID,
                            DisplayName = x.DisplayName,
                            StatusID = x.StatusID,
                            StatusName = x.MemberStatus!.StatusName,
                            ViolationCount = x.ViolationCount,
                            CreatedAt = x.CreatedAt,
                            CreatorID = x.CreatorProfile != null ? x.CreatorProfile.CreatorID : null,
                            BrandName = x.CreatorProfile != null ? x.CreatorProfile.BrandName : null,
                            RoleIDs = x.MemberRoles
                                .Select(r => r.RoleID)
                                .ToList()
                        })
                        .ToListAsync();

                    return new VMAdminMemberIndex
                    {
                        Mode = mode,
                        SearchMemberId = memberId,
                        Items = riskItems,
                        Page = 1,
                        PageSize = pageSize,
                        TotalCount = riskItems.Count
                    };

                case "admin":
                    query = query
                        .Where(x => x.MemberRoles.Any(r => r.RoleID == "03" || r.RoleID == "04"))
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenBy(x => x.MemberID);

                    var adminItems = await query
                        .Select(x => new VMAdminMemberListItem
                        {
                            MemberID = x.MemberID,
                            DisplayName = x.DisplayName,
                            StatusID = x.StatusID,
                            StatusName = x.MemberStatus!.StatusName,
                            ViolationCount = x.ViolationCount,
                            CreatedAt = x.CreatedAt,
                            CreatorID = x.CreatorProfile != null ? x.CreatorProfile.CreatorID : null,
                            BrandName = x.CreatorProfile != null ? x.CreatorProfile.BrandName : null,
                            RoleIDs = x.MemberRoles
                                .Select(r => r.RoleID)
                                .ToList()
                        })
                        .ToListAsync();

                    return new VMAdminMemberIndex
                    {
                        Mode = mode,
                        SearchMemberId = memberId,
                        Items = adminItems,
                        Page = 1,
                        PageSize = pageSize,
                        TotalCount = adminItems.Count
                    };

                case "creator":
                    query = query.Where(x => x.CreatorProfile != null);

                    if (!string.IsNullOrWhiteSpace(memberId))
                    {
                        query = query.Where(x => x.MemberID == memberId);
                    }

                    query = query
                        .OrderByDescending(x => x.CreatorProfile!.CreatedAt)
                        .ThenBy(x => x.MemberID);

                    var creatorTotal = await query.CountAsync();

                    var creatorItems = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(x => new VMAdminMemberListItem
                        {
                            MemberID = x.MemberID,
                            DisplayName = x.DisplayName,
                            StatusID = x.StatusID,
                            StatusName = x.MemberStatus!.StatusName,
                            ViolationCount = x.ViolationCount,
                            CreatedAt = x.CreatedAt,
                            CreatorID = x.CreatorProfile != null ? x.CreatorProfile.CreatorID : null,
                            BrandName = x.CreatorProfile != null ? x.CreatorProfile.BrandName : null,
                            RoleIDs = x.MemberRoles
                                .Select(r => r.RoleID)
                                .ToList()
                        })
                        .ToListAsync();

                    return new VMAdminMemberIndex
                    {
                        Mode = mode,
                        SearchMemberId = memberId,
                        Items = creatorItems,
                        Page = page,
                        PageSize = pageSize,
                        TotalCount = creatorTotal
                    };

                case "all":
                default:
                    if (!string.IsNullOrWhiteSpace(memberId))
                    {
                        query = query.Where(x => x.MemberID == memberId);
                    }

                    query = query
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenBy(x => x.MemberID);

                    var total = await query.CountAsync();

                    var items = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(x => new VMAdminMemberListItem
                        {
                            MemberID = x.MemberID,
                            DisplayName = x.DisplayName,
                            StatusID = x.StatusID,
                            StatusName = x.MemberStatus!.StatusName,
                            ViolationCount = x.ViolationCount,
                            CreatedAt = x.CreatedAt,
                            CreatorID = x.CreatorProfile != null ? x.CreatorProfile.CreatorID : null,
                            BrandName = x.CreatorProfile != null ? x.CreatorProfile.BrandName : null,
                            RoleIDs = x.MemberRoles
                                .Select(r => r.RoleID)
                                .ToList()
                        })
                        .ToListAsync();

                    return new VMAdminMemberIndex
                    {
                        Mode = mode,
                        SearchMemberId = memberId,
                        Items = items,
                        Page = page,
                        PageSize = pageSize,
                        TotalCount = total
                    };
            }
        }

        public async Task<VMAdminMemberDetail?> GetDetailAsync(string memberId, string mode = "all")
        {
            mode = (mode ?? "all").Trim().ToLower();

            var member = await _context.Members
                .AsNoTracking()
                .Include(x => x.MemberStatus)
                .Include(x => x.Privacy)
                .Include(x => x.MemberRoles)
                    .ThenInclude(x => x.Role)
                .Include(x => x.CreatorProfile)
                    .ThenInclude(x => x!.CreatorProfileStatus)
                .FirstOrDefaultAsync(x => x.MemberID == memberId);

            if (member == null)
                return null;

            if (mode == "risk" && member.ViolationCount <= 5)
                return null;

            if (mode == "admin" && !member.MemberRoles.Any(r => r.RoleID == "03" || r.RoleID == "04"))
                return null;

            if (mode == "creator" && member.CreatorProfile == null)
                return null;

            return new VMAdminMemberDetail
            {
                MemberID = member.MemberID,
                DisplayName = member.DisplayName,
                StatusID = member.StatusID,
                StatusName = member.MemberStatus!.StatusName,
                MaliciousReportCount = member.MaliciousReportCount,
                ReportBanUntil = member.ReportBanUntil,
                ViolationCount = member.ViolationCount,
                CreatedAt = member.CreatedAt,
                Email = member.Privacy?.Email,
                Phone = member.Privacy?.Phone,
                RoleIDs = member.MemberRoles.Select(r => r.RoleID).ToList(),
                RoleNames = member.MemberRoles.Select(r => r.Role.RoleName).ToList(),

                CreatorID = member.CreatorProfile?.CreatorID,
                CreatorStatusID = member.CreatorProfile?.StatusID,
                CreatorStatusName = member.CreatorProfile?.CreatorProfileStatus?.StatusName,
                BrandName = member.CreatorProfile?.BrandName,
                BrandIntro = member.CreatorProfile?.BrandIntro,
                StartDate = member.CreatorProfile?.StartDate,
                BankCode = member.CreatorProfile?.BankCode,
                BankAccount = member.CreatorProfile?.BankAccount,
                CreatorCreatedAt = member.CreatorProfile?.CreatedAt,
                UpdatedAt = member.CreatorProfile?.UpdatedAt
            };
        }

        public async Task<(bool ok, string? message)> SuspendAsync(string memberId, string adminMemberId)
        {
            var member = await _context.Members
                .Include(x => x.MemberRoles)
                .FirstOrDefaultAsync(x => x.MemberID == memberId);

            if (member == null)
                return (false, "會員不存在");

            if (member.MemberID == adminMemberId)
                return (false, "不能停權自己的帳號");

            if (member.MemberRoles.Any(r => r.RoleID == "04"))
                return (false, "不可停權超級管理者");

            if (member.StatusID == MEMBER_SUSPENDED)
                return (false, "此會員目前已是停權狀態");

            member.StatusID = MEMBER_SUSPENDED;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool ok, string? message)> ActivateAsync(string memberId, string adminMemberId)
        {
            var member = await _context.Members
                .Include(x => x.MemberRoles)
                .FirstOrDefaultAsync(x => x.MemberID == memberId);

            if (member == null)
                return (false, "會員不存在");

            if (member.StatusID == MEMBER_ACTIVE)
                return (false, "此會員目前已是啟用狀態");

            member.StatusID = MEMBER_ACTIVE;

            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}