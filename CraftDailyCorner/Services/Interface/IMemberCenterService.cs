using CraftDailyCorner.ViewModels.Member;

namespace CraftDailyCorner.Services.Interface
{
    public interface IMemberCenterService
    {
        VMMemberDashboard GetDashboard(string memberId);

        VMEditProfile GetProfile(string memberId);

        void UpdateProfile(string memberId, VMEditProfile vm);
    }
}