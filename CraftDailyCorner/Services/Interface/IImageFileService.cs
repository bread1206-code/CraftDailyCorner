namespace CraftDailyCorner.Services.Interface
{
    public interface IImageFileService
    {
        void DeletePortfolioImage(string imageName);
        void DeleteProductImage(string imageName);
        void DeleteCreatorPostImage(string imageName);
    }
}
