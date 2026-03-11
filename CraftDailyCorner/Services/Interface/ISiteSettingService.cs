namespace CraftDailyCorner.Services.Interface
{
    public interface ISiteSettingService
    {
        Task<string?> GetStringAsync(string key);

        Task<int> GetIntAsync(string key);

        Task<bool> GetBoolAsync(string key);
    }
}