using CraftDailyCorner.DTOs;
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
    }
}