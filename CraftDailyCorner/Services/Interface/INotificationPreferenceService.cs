using CraftDailyCorner.ViewModels.Notification;

namespace CraftDailyCorner.Services.Interface
{
    public interface INotificationPreferenceService
    {
        VMNotificationPreference GetPreference(string memberId);

        void UpdatePreference(string memberId, VMNotificationPreference vm);
    }
}