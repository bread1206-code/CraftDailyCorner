using CraftDailyCorner.Services.Interface;

namespace CraftDailyCorner.Services
{
    public class ImageFileService : IImageFileService
    {
        private readonly IWebHostEnvironment _env;

        public ImageFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // 作品集圖片刪除
        public void DeletePortfolioImage(string creatorId, string imageName)
        {
            DeleteImage(imageName, "06Portfolio", creatorId);
        }

        // 商品圖片刪除
        public void DeleteProductImage(string creatorId, string imageName)
        {
            DeleteImage(imageName, "04ProductImage", creatorId);
        }

        // 創作日誌圖片刪除
        public void DeleteCreatorPostImage(string creatorId, string imageName)
        {
            DeleteImage(imageName, "05CreatorPost", creatorId);
        }

        // 共用刪除邏輯
        private void DeleteImage(string imageName, string folderName, string creatorId)
        {
            try
            {
                var largePath = Path.Combine(
                    _env.WebRootPath,
                    "Photos",
                    folderName,
                    creatorId,
                    "Large",
                    imageName + ".webp");

                var mediumPath = Path.Combine(
                    _env.WebRootPath,
                    "Photos",
                    folderName,
                    creatorId,
                    "Medium",
                    imageName + ".webp");

                if (File.Exists(largePath))
                    File.Delete(largePath);

                if (File.Exists(mediumPath))
                    File.Delete(mediumPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("刪除圖片檔案失敗: " + ex.Message);
            }
        }
    }
}