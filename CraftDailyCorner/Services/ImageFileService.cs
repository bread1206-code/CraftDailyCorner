using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
            public void DeletePortfolioImage(string imageName)
            {
                DeleteImage(imageName, "06Portfolio");
            }

            // 商品圖片刪除
            public void DeleteProductImage(string imageName)
            {
                DeleteImage(imageName, "04ProductImage");
            }

            // 創作日誌圖片刪除
            public void DeleteCreatorPostImage(string imageName)
            {
                DeleteImage(imageName, "05CreatorPost");
            }

            //共用刪除邏輯
            private void DeleteImage(string imageName, string folderName)
            {
                try
                {
                    var largePath = Path.Combine(
                        _env.WebRootPath,
                        "Photos",
                        folderName,
                        "Large",
                        imageName + ".png");

                    var mediumPath = Path.Combine(
                        _env.WebRootPath,
                        "Photos",
                        folderName,
                        "Medium",
                        imageName + ".png");


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

