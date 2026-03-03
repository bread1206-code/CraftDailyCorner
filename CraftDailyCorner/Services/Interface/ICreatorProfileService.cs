using CraftDailyCorner.ViewModels.Creator;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorProfileService
    {
        Task<VMCreatorBrandEdit?> GetBrandEditAsync(string creatorId);
        Task UpdateBrandAsync(string creatorId, VMCreatorBrandEdit vm);
    }
}
