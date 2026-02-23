using CraftDailyCorner.DTOs;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPortfolioItemService
    {
        Task UploadAsync(string portfolioId,string creatorId,List<IFormFile> files);

        Task<string> DeleteAsync(int itemId,string creatorId);

        Task UpdateSortAsync(int itemId,byte sortOrder,string creatorId);

        Task UpdateSortBatchAsync(List<SortUpdateDTO> items,string creatorId);
    }
}
