using CraftDailyCorner.ViewModels.Member;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAccountService
    {
        Task<string> RegisterMemberAsync(VMRegister vm);

    }
}
