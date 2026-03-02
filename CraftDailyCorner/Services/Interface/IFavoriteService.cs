using CraftDailyCorner.ViewModels.Member;
using CraftDailyCorner.ViewModels.Product;

namespace CraftDailyCorner.Services.Interface
{
    public interface IFavoriteService
    {
        bool IsFavorite(string memberId, string productId);
        void AddFavorite(string memberId, string productId);
        void RemoveFavorite(string memberId, string productId);
        List<VMProductListItem> GetFavoriteProducts(string memberId);
        List<VMFavoriteProductItem> GetMyFavorites(string memberId);
    }
}
