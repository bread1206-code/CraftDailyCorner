namespace CraftDailyCorner.Services.Interface
{
    public interface IImageFileService
    {
        void DeletePortfolioImage(string creatorId, string imageName);
        void DeleteProductImage(string creatorId, string imageName);
        void DeleteCreatorPostImage(string creatorId, string imageName);
    }
}