using CraftDailyCorner.Services;

namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public class DemoSeedImageHelper
    {
        private readonly IImageUploadService _imageUploadService;

        public DemoSeedImageHelper(IImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }

        public string UploadCreatorBrandImage(
            string sourceFilePath,
            string creatorId)
        {
            var imageGuid = Guid.NewGuid().ToString();

            _imageUploadService.UploadFromSeed(
                seedFolder: "03CreatorBrand",
                sourceFile: sourceFilePath,
                fileNameWithoutExt: imageGuid,
                sizes: ImageSizePresets.Creator,
                entitySubFolder: creatorId
            );

            return imageGuid;
        }

        public string UploadCreatorPortfolioSampleImage(
            string sourceFilePath,
            string creatorId)
        {
            var imageGuid = Guid.NewGuid().ToString();

            _imageUploadService.UploadFromSeed(
                seedFolder: "02CreatorApplication",
                sourceFile: sourceFilePath,
                fileNameWithoutExt: imageGuid,
                sizes: ImageSizePresets.CreatorApplication,
                entitySubFolder: creatorId
            );

            return imageGuid;
        }

        public string UploadCreatorPostImage(
            string sourceFilePath,
            string creatorId)
        {
            var imageGuid = Guid.NewGuid().ToString();

            _imageUploadService.UploadFromSeed(
                seedFolder: "05CreatorPost",
                sourceFile: sourceFilePath,
                fileNameWithoutExt: imageGuid,
                sizes: ImageSizePresets.Post,
                entitySubFolder: creatorId
            );

            return imageGuid;
        }

        public string UploadProductImage(
            string sourceFilePath,
            string creatorId)
        {
            var imageGuid = Guid.NewGuid().ToString();

            _imageUploadService.UploadFromSeed(
                seedFolder: "04ProductImage",
                sourceFile: sourceFilePath,
                fileNameWithoutExt: imageGuid,
                sizes: ImageSizePresets.Product,
                entitySubFolder: creatorId
            );

            return imageGuid;
        }

        public string UploadPortfolioImage(
            string sourceFilePath,
            string creatorId)
        {
            var imageGuid = Guid.NewGuid().ToString();

            _imageUploadService.UploadFromSeed(
                seedFolder: "06Portfolio",
                sourceFile: sourceFilePath,
                fileNameWithoutExt: imageGuid,
                sizes: ImageSizePresets.Portfolio,
                entitySubFolder: creatorId
            );

            return imageGuid;
        }

        public List<string> GetOrderedFiles(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("資料夾路徑不可為空");

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"找不到資料夾：{folderPath}");

            return Directory.GetFiles(folderPath)
                .OrderBy(Path.GetFileName)
                .ToList();
        }

        public string GetFileByIndex(List<string> orderedFiles, int index, string folderName)
        {
            if (orderedFiles == null || !orderedFiles.Any())
                throw new Exception($"{folderName} 沒有任何圖片");

            if (index < 0 || index >= orderedFiles.Count)
                throw new Exception($"{folderName} 圖片數量不足，索引超出範圍：{index}");

            return orderedFiles[index];
        }
    }
}