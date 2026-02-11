using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.DTOs;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPortfolioService
    {
        // ===== 前台 =====
        Task<VMPortfolioIndex> GetPortfolioIndexAsync(VMPortfolioIndexQuery query);
        Task<VMPortfolioDetail?> GetPublicPortfolioDetailAsync(string portfolioId);

        // ===== 後台 =====
        Task<List<VMCreatorPortfolioListItem>> GetCreatorPortfoliosAsync(string creatorId);
        Task<VMCreatorPortfolioEdit?> GetEditDataAsync(string portfolioId, string creatorId);

        Task CreateAsync(CreateCreatorPortfolioDTO dto, string creatorId);
        Task UpdateAsync(UpdateCreatorPortfolioDTO dto, string creatorId);
        Task SoftDeleteAsync(string portfolioId, string creatorId);
    }
}
