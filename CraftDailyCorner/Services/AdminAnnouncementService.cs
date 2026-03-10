using CraftDailyCorner.Areas.Admin.ViewModels.Announcement;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminAnnouncementService : IAdminAnnouncementService
    {
        private readonly CraftDailyCornerContext _context;

        private const byte STATUS_DRAFT = 1;
        private const byte STATUS_ACTIVE = 2;
        private const byte STATUS_INACTIVE = 3;

        public AdminAnnouncementService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminAnnouncementIndex> GetIndexAsync()
        {
            var items = await _context.PlatformAnnouncements
                .AsNoTracking()
                .Include(x => x.PlatformAnnouncementStatus)
                .Include(x => x.Member)
                .OrderByDescending(x => x.StatusID == STATUS_ACTIVE)
                .ThenByDescending(x => x.PublishedAt ?? x.CreatedAt)
                .ThenByDescending(x => x.AnnouncementID)
                .Select(x => new VMAdminAnnouncementIndexItem
                {
                    AnnouncementID = x.AnnouncementID,
                    Title = x.Title,
                    AudienceType = (byte)x.AudienceType,
                    AudienceName = GetAudienceName((byte)x.AudienceType),
                    StatusID = x.StatusID,
                    StatusName = x.PlatformAnnouncementStatus.StatusName,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.Member.DisplayName,
                    PublishedAt = x.PublishedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return new VMAdminAnnouncementIndex
            {
                Items = items
            };
        }

        public async Task<VMAdminAnnouncementUpsert> GetCreateVmAsync(string currentMemberId, bool isSuperAdmin)
        {
            var vm = new VMAdminAnnouncementUpsert
            {
                StatusID = STATUS_DRAFT,
                AudienceOptions = await BuildAudienceOptionsAsync(isSuperAdmin),
                StatusOptions = await BuildStatusOptionsAsync()
            };

            return vm;
        }

        public async Task<VMAdminAnnouncementUpsert?> GetEditVmAsync(int id, string currentMemberId, bool isSuperAdmin)
        {
            var entity = await _context.PlatformAnnouncements
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AnnouncementID == id);

            if (entity == null)
                return null;

            // 一般管理者不可編輯「僅管理者」公告
            if (!isSuperAdmin && entity.AudienceType == AnnouncementAudienceType.AdminsOnly)
                return null;

            return new VMAdminAnnouncementUpsert
            {
                AnnouncementID = entity.AnnouncementID,
                Title = entity.Title,
                Content = entity.Content,
                AudienceType = (byte)entity.AudienceType,
                StatusID = entity.StatusID,
                AudienceOptions = await BuildAudienceOptionsAsync(isSuperAdmin),
                StatusOptions = await BuildStatusOptionsAsync()
            };
        }

        public async Task<VMAdminAnnouncementDetail?> GetDetailAsync(int id)
        {
            var entity = await _context.PlatformAnnouncements
                .AsNoTracking()
                .Include(x => x.PlatformAnnouncementStatus)
                .Include(x => x.Member)
                .FirstOrDefaultAsync(x => x.AnnouncementID == id);

            if (entity == null)
                return null;

            string? updatedByName = null;
            if (!string.IsNullOrWhiteSpace(entity.UpdatedBy))
            {
                updatedByName = await _context.Members
                    .AsNoTracking()
                    .Where(x => x.MemberID == entity.UpdatedBy)
                    .Select(x => x.DisplayName)
                    .FirstOrDefaultAsync();
            }

            return new VMAdminAnnouncementDetail
            {
                AnnouncementID = entity.AnnouncementID,
                Title = entity.Title,
                Content = entity.Content,
                AudienceType = (byte)entity.AudienceType,
                AudienceName = GetAudienceName((byte)entity.AudienceType),
                StatusID = entity.StatusID,
                StatusName = entity.PlatformAnnouncementStatus.StatusName,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                CreatedByName = entity.Member.DisplayName,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,
                UpdatedByName = updatedByName,
                PublishedAt = entity.PublishedAt
            };
        }

        public async Task<int> CreateAsync(VMAdminAnnouncementUpsert vm, string currentMemberId, bool isSuperAdmin)
        {
            ValidateAudiencePermission(vm.AudienceType, isSuperAdmin);

            var now = DateTime.Now;

            var entity = new PlatformAnnouncement
            {
                Title = vm.Title.Trim(),
                Content = vm.Content.Trim(),
                AudienceType = (AnnouncementAudienceType)vm.AudienceType,
                StatusID = vm.StatusID,
                CreatedAt = now,
                CreatedBy = currentMemberId,
                UpdatedAt = now,
                UpdatedBy = currentMemberId,
                PublishedAt = vm.StatusID == STATUS_ACTIVE ? now : null
            };

            _context.PlatformAnnouncements.Add(entity);
            await _context.SaveChangesAsync();

            return entity.AnnouncementID;
        }

        public async Task<bool> UpdateAsync(VMAdminAnnouncementUpsert vm, string currentMemberId, bool isSuperAdmin)
        {
            if (vm.AnnouncementID == null)
                return false;

            ValidateAudiencePermission(vm.AudienceType, isSuperAdmin);

            var entity = await _context.PlatformAnnouncements
                .FirstOrDefaultAsync(x => x.AnnouncementID == vm.AnnouncementID.Value);

            if (entity == null)
                return false;

            // 一般管理者不可修改既有的「僅管理者」公告
            if (!isSuperAdmin && entity.AudienceType == AnnouncementAudienceType.AdminsOnly)
                return false;

            var wasActive = entity.StatusID == STATUS_ACTIVE;

            entity.Title = vm.Title.Trim();
            entity.Content = vm.Content.Trim();
            entity.AudienceType = (AnnouncementAudienceType)vm.AudienceType;
            entity.StatusID = vm.StatusID;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = currentMemberId;

            // 第一次切到 Active 時補 PublishedAt
            if (!wasActive && vm.StatusID == STATUS_ACTIVE)
            {
                entity.PublishedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool ok, string? message)> ActivateAsync(int id, string currentMemberId, bool isSuperAdmin)
        {
            var entity = await _context.PlatformAnnouncements
                .FirstOrDefaultAsync(x => x.AnnouncementID == id);

            if (entity == null)
                return (false, "找不到公告資料");

            if (!isSuperAdmin && entity.AudienceType == AnnouncementAudienceType.AdminsOnly)
                return (false, "只有超級管理者可以啟用管理者公告");

            entity.StatusID = STATUS_ACTIVE;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = currentMemberId;

            if (!entity.PublishedAt.HasValue)
                entity.PublishedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool ok, string? message)> InactivateAsync(int id, string currentMemberId, bool isSuperAdmin)
        {
            var entity = await _context.PlatformAnnouncements
                .FirstOrDefaultAsync(x => x.AnnouncementID == id);

            if (entity == null)
                return (false, "找不到公告資料");

            if (!isSuperAdmin && entity.AudienceType == AnnouncementAudienceType.AdminsOnly)
                return (false, "只有超級管理者可以停用管理者公告");

            entity.StatusID = STATUS_INACTIVE;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = currentMemberId;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        private void ValidateAudiencePermission(byte audienceType, bool isSuperAdmin)
        {
            if (!Enum.IsDefined(typeof(AnnouncementAudienceType), (int)audienceType))
                throw new ArgumentException("受眾類型不正確");

            if (!isSuperAdmin && audienceType == (byte)AnnouncementAudienceType.AdminsOnly)
                throw new ArgumentException("只有超級管理者可以發布「僅管理者」公告");
        }

        private async Task<SelectList> BuildAudienceOptionsAsync(bool isSuperAdmin)
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = ((byte)AnnouncementAudienceType.AllMembers).ToString(),
                    Text = "所有會員"
                },
                new SelectListItem
                {
                    Value = ((byte)AnnouncementAudienceType.CreatorsOnly).ToString(),
                    Text = "僅創作者"
                }
            };

            if (isSuperAdmin)
            {
                items.Add(new SelectListItem
                {
                    Value = ((byte)AnnouncementAudienceType.AdminsOnly).ToString(),
                    Text = "僅管理者"
                });
            }

            return new SelectList(items, "Value", "Text");
        }

        private async Task<SelectList> BuildStatusOptionsAsync()
        {
            var items = await _context.PlatformAnnouncementStatuses
                .AsNoTracking()
                .OrderBy(x => x.StatusID)
                .Select(x => new SelectListItem
                {
                    Value = x.StatusID.ToString(),
                    Text = x.StatusName
                })
                .ToListAsync();

            return new SelectList(items, "Value", "Text");
        }

        private static string GetAudienceName(byte audienceType)
        {
            return audienceType switch
            {
                (byte)AnnouncementAudienceType.AllMembers => "所有會員",
                (byte)AnnouncementAudienceType.CreatorsOnly => "僅創作者",
                (byte)AnnouncementAudienceType.AdminsOnly => "僅管理者",
                _ => "未知"
            };
        }
    }
}