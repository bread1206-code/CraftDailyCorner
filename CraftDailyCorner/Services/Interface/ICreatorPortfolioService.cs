using CraftDailyCorner.DTOs;
using CraftDailyCorner.ViewModels.CreatorPortfolio;
using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPortfolioService
    {
        // 前台
        Task<VMPortfolioIndex> GetPortfolioIndexAsync(
            VMPortfolioIndexQuery query,
            string? currentMemberId);

        Task<VMPortfolioDetail?> GetPublicPortfolioDetailAsync(
            string portfolioId,
            string? currentMemberId);

        Task<bool> CanViewPortfolioAsync(string portfolioId, string? memberId);

        // 後台
        Task<List<VMCreatorPortfolioListItem>> GetCreatorPortfoliosAsync(string creatorId);
        Task<VMCreatorPortfolioEdit?> GetEditDataAsync(string portfolioId, string creatorId);

        // 建立 / 更新 / 刪除
        Task CreateAsync(CreateCreatorPortfolioDTO dto, string creatorId, List<IFormFile> files);
        Task UpdateAsync(UpdateCreatorPortfolioDTO dto, string creatorId);
        Task SoftDeleteAsync(string portfolioId, string creatorId);
    }
}