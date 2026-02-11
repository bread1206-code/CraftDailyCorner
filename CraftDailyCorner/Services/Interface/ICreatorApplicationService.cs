using CraftDailyCorner.ViewModels.Front.CreatorApplication;
using CraftDailyCorner.ViewModels.Front.DTOs;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorApplicationService
    {
        Task<object> GetApplyPageAsync(string memberId);

        Task<bool> HasPendingAsync(string memberId);

        Task CreateAsync(CreatorApplicationCreateDTO dto);
    }
}
