namespace CraftDailyCorner.Services.Interface
{
    public interface ISoftDeleteCleanupTask
    {
        Task CleanupAsync(IServiceProvider serviceProvider);
    }
}
