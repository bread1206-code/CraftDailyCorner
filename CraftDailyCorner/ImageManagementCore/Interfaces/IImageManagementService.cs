using CraftDailyCorner.ImageManagementCore.Interfaces;

namespace CraftDailyCorner.ImageManagementCore.Services.Interfaces
{
    public interface IImageManagementService
    {
        string EntityType { get; }
        //圖片上限（可為 null 表示不限）
        int? MaxImageCount { get; }

        //提示文字
        string? HintMessage { get; }
        Task<List<IEntityImage>> GetImagesAsync(string entityId);

        Task AddWithUploadAsync(
            IFormFile file,
            string entityId,
            string creatorId);

        Task DeleteWithValidationAsync(
            long imageId,
            string creatorId);

        Task UpdateSortWithValidationAsync(
            string entityId,
            List<long> orderedIds,
            string creatorId);
    }
}