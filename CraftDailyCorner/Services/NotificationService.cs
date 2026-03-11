using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Notification;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class NotificationService : INotificationService
    {
        private readonly CraftDailyCornerContext _context;

        public NotificationService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateNotificationDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            ValidateCreateDto(dto);

            var shouldReceive = await ShouldReceiveNotificationAsync(dto.MemberID, dto.NotificationType);
            if (!shouldReceive)
                return;

            var entity = new NotificationEvent
            {
                MemberID = dto.MemberID,
                NotificationType = dto.NotificationType,
                Title = dto.Title.Trim(),
                Content = dto.Content.Trim(),
                LinkUrl = string.IsNullOrWhiteSpace(dto.LinkUrl) ? null : dto.LinkUrl.Trim(),
                RelatedEntityType = string.IsNullOrWhiteSpace(dto.RelatedEntityType) ? null : dto.RelatedEntityType.Trim(),
                RelatedEntityId = string.IsNullOrWhiteSpace(dto.RelatedEntityId) ? null : dto.RelatedEntityId.Trim(),
                IsRead = false,
                ReadAt = null,
                CreatedAt = DateTime.Now
            };

            _context.NotificationEvents.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBatchAsync(IEnumerable<CreateNotificationDTO> dtos)
        {
            if (dtos == null)
                throw new ArgumentNullException(nameof(dtos));

            var dtoList = dtos.ToList();
            if (!dtoList.Any())
                return;

            foreach (var dto in dtoList)
            {
                ValidateCreateDto(dto);
            }

            var memberIds = dtoList
                .Select(x => x.MemberID)
                .Distinct()
                .ToList();

            var preferences = await _context.NotificationPreferences
                .Where(x => memberIds.Contains(x.MemberID))
                .ToListAsync();

            var eventsToAdd = new List<NotificationEvent>();

            foreach (var dto in dtoList)
            {
                var shouldReceive = ShouldReceiveNotification(dto.MemberID, dto.NotificationType, preferences);

                if (!shouldReceive)
                    continue;

                eventsToAdd.Add(new NotificationEvent
                {
                    MemberID = dto.MemberID,
                    NotificationType = dto.NotificationType,
                    Title = dto.Title.Trim(),
                    Content = dto.Content.Trim(),
                    LinkUrl = string.IsNullOrWhiteSpace(dto.LinkUrl) ? null : dto.LinkUrl.Trim(),
                    RelatedEntityType = string.IsNullOrWhiteSpace(dto.RelatedEntityType) ? null : dto.RelatedEntityType.Trim(),
                    RelatedEntityId = string.IsNullOrWhiteSpace(dto.RelatedEntityId) ? null : dto.RelatedEntityId.Trim(),
                    IsRead = false,
                    ReadAt = null,
                    CreatedAt = DateTime.Now
                });
            }

            if (!eventsToAdd.Any())
                return;

            _context.NotificationEvents.AddRange(eventsToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VMNotificationItem>> GetRecentAsync(string memberId, int count = 5)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            if (count <= 0)
                count = 5;

            return await _context.NotificationEvents
                .AsNoTracking()
                .Where(x => x.MemberID == memberId)
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.EventID)
                .Take(count)
                .Select(x => new VMNotificationItem
                {
                    EventID = x.EventID,
                    NotificationType = x.NotificationType,
                    Title = x.Title,
                    Content = x.Content,
                    LinkUrl = x.LinkUrl,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            return await _context.NotificationEvents
                .CountAsync(x => x.MemberID == memberId && !x.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(long eventId, string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            var entity = await _context.NotificationEvents
                .FirstOrDefaultAsync(x => x.EventID == eventId && x.MemberID == memberId);

            if (entity == null)
                return false;

            if (entity.IsRead)
                return true;

            entity.IsRead = true;
            entity.ReadAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> MarkAllAsReadAsync(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                throw new ArgumentException("memberId 不可為空");

            var entities = await _context.NotificationEvents
                .Where(x => x.MemberID == memberId && !x.IsRead)
                .ToListAsync();

            if (!entities.Any())
                return 0;

            var now = DateTime.Now;

            foreach (var item in entities)
            {
                item.IsRead = true;
                item.ReadAt = now;
            }

            await _context.SaveChangesAsync();
            return entities.Count;
        }

        private void ValidateCreateDto(CreateNotificationDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.MemberID))
                throw new ArgumentException("MemberID 不可為空");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("通知標題不可為空");

            if (dto.Title.Trim().Length > 100)
                throw new ArgumentException("通知標題不可超過 100 字");

            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("通知內容不可為空");

            if (!Enum.IsDefined(typeof(NotificationType), dto.NotificationType))
                throw new ArgumentException("通知類型不正確");

            if (!string.IsNullOrWhiteSpace(dto.LinkUrl) && dto.LinkUrl.Trim().Length > 255)
                throw new ArgumentException("通知連結不可超過 255 字");

            if (!string.IsNullOrWhiteSpace(dto.RelatedEntityType) && dto.RelatedEntityType.Trim().Length > 30)
                throw new ArgumentException("RelatedEntityType 不可超過 30 字");

            if (!string.IsNullOrWhiteSpace(dto.RelatedEntityId) && dto.RelatedEntityId.Trim().Length > 36)
                throw new ArgumentException("RelatedEntityId 不可超過 36 字");
        }

        private async Task<bool> ShouldReceiveNotificationAsync(string memberId, NotificationType notificationType)
        {
            var preference = await _context.NotificationPreferences
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MemberID == memberId &&
                    x.NotificationType == notificationType);

            // 若尚未建立偏好資料，預設視為開啟
            return preference?.IsActive ?? true;
        }

        private bool ShouldReceiveNotification(
            string memberId,
            NotificationType notificationType,
            List<NotificationPreference> preferences)
        {
            var preference = preferences
                .FirstOrDefault(x => x.MemberID == memberId && x.NotificationType == notificationType);

            return preference?.IsActive ?? true;
        }
    }
}