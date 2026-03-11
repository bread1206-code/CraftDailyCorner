using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.ViewModels.Notification;

namespace CraftDailyCorner.Services.Interface
{
    public interface INotificationService
    {
        Task CreateAsync(CreateNotificationDTO dto);

        Task CreateBatchAsync(IEnumerable<CreateNotificationDTO> dtos);

        Task<List<VMNotificationItem>> GetRecentAsync(string memberId, int count = 5);

        Task<int> GetUnreadCountAsync(string memberId);

        Task<bool> MarkAsReadAsync(long eventId, string memberId);

        Task<int> MarkAllAsReadAsync(string memberId);

        Task<VMNotificationIndex> GetPagedAsync(
            string memberId,
            int page = 1,
            int pageSize = 10,
            bool unreadOnly = false,
            NotificationFilterType filterType = NotificationFilterType.All);
    }
}