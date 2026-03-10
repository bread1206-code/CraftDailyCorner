using CraftDailyCorner.ViewModels.Announcement;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAnnouncementService
    {
        Task<VMAnnouncementList> GetListAsync(string memberId, bool isCreator, bool isAdmin);

        Task<VMAnnouncementDetail?> GetDetailAsync(int id, string memberId, bool isCreator, bool isAdmin);

        Task<List<VMAnnouncementListItem>> GetTopAnnouncementsAsync(string memberId, bool isCreator, bool isAdmin, int count = 3);
    }
}