using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using CraftDailyCorner.Services;
namespace CraftDailyCorner.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _env;

        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void UploadSeedImage(
            string seedFolder,
            string sourceFile,
            string fileNameWithoutExt,
            List<ImageSizeOption> sizes
        )
        {
            // 來源圖片資料夾
            string seedPhotoPath = Path.Combine(
                _env.ContentRootPath,   // 專案根目錄
                "Seed",
                "SeedPhotos",
                seedFolder
            );

            // 目標上傳資料夾
            string basePhotoPath = Path.Combine(
                _env.WebRootPath,      // wwwroot
                "Photos",
                seedFolder
            );

            using Image image = Image.Load(sourceFile);

            foreach (var size in sizes)
            {
                string folderPath = Path.Combine(basePhotoPath, size.FolderName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var options = new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(size.Width, size.Height),
                    Mode = ResizeMode.Crop,
                    Position = AnchorPositionMode.Center
                };

                using Image resized = image.Clone(ctx => ctx.Resize(options));

                string destFile = Path.Combine(
                    folderPath,
                    $"{fileNameWithoutExt}.png"
                );

                resized.Save(destFile, new PngEncoder());
            }
        }
    }
}