using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Announcement;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly CraftDailyCornerContext _context;

        private const byte STATUS_ACTIVE = 2;

        public AnnouncementService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAnnouncementList> GetListAsync(string memberId, bool isCreator, bool isAdmin)
        {
            var audienceTypes = GetAvailableAudienceTypes(isCreator, isAdmin);

            var items = await _context.PlatformAnnouncements
                .AsNoTracking()
                .Where(x => x.StatusID == STATUS_ACTIVE &&
                            audienceTypes.Contains((byte)x.AudienceType))
                .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
                .ThenByDescending(x => x.AnnouncementID)
                .Select(x => new VMAnnouncementListItem
                {
                    AnnouncementID = x.AnnouncementID,
                    Title = x.Title,
                    ContentPreview = x.Content.Length > 120
                        ? x.Content.Substring(0, 120) + "..."
                        : x.Content,
                    PublishedAt = x.PublishedAt,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new VMAnnouncementList
            {
                Items = items
            };
        }

        public async Task<VMAnnouncementDetail?> GetDetailAsync(int id, string memberId, bool isCreator, bool isAdmin)
        {
            var audienceTypes = GetAvailableAudienceTypes(isCreator, isAdmin);

            var entity = await _context.PlatformAnnouncements
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.AnnouncementID == id &&
                    x.StatusID == STATUS_ACTIVE &&
                    audienceTypes.Contains((byte)x.AudienceType));

            if (entity == null)
                return null;

            return new VMAnnouncementDetail
            {
                AnnouncementID = entity.AnnouncementID,
                Title = entity.Title,
                Content = entity.Content,
                PublishedAt = entity.PublishedAt,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<List<VMAnnouncementListItem>> GetTopAnnouncementsAsync(string memberId, bool isCreator, bool isAdmin, int count = 3)
        {
            var audienceTypes = GetAvailableAudienceTypes(isCreator, isAdmin);

            return await _context.PlatformAnnouncements
                .AsNoTracking()
                .Where(x => x.StatusID == STATUS_ACTIVE &&
                            audienceTypes.Contains((byte)x.AudienceType))
                .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
                .ThenByDescending(x => x.AnnouncementID)
                .Take(count)
                .Select(x => new VMAnnouncementListItem
                {
                    AnnouncementID = x.AnnouncementID,
                    Title = x.Title,
                    ContentPreview = x.Content.Length > 60
                        ? x.Content.Substring(0, 60) + "..."
                        : x.Content,
                    PublishedAt = x.PublishedAt,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        private List<byte> GetAvailableAudienceTypes(bool isCreator, bool isAdmin)
        {
            var result = new List<byte>
            {
                (byte)AnnouncementAudienceType.AllMembers
            };

            if (isCreator)
            {
                result.Add((byte)AnnouncementAudienceType.CreatorsOnly);
            }

            if (isAdmin)
            {
                result.Add((byte)AnnouncementAudienceType.AdminsOnly);
            }

            return result;
        }
    }
}