using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
namespace CraftDailyCorner.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _env;

        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string UploadImage(IFormFile? file,string? seedSourcePath,string folderName,
            List<ImageSizeOption> sizes,string? entityId = null)
        {
            // Seed 圖片
            if (file == null && !string.IsNullOrEmpty(seedSourcePath))
            {
                UploadFromSeed(
                    seedFolder: folderName,
                    sourceFile: seedSourcePath,
                    fileNameWithoutExt: entityId ?? Guid.NewGuid().ToString(),
                    sizes: sizes
                );
                return entityId ?? string.Empty;
            }

            // 使用者上傳
            if (file != null)
            {
                return UploadFromFormFile(file, folderName, sizes, entityId);
            }

            throw new InvalidOperationException("無有效圖片來源");
        }

        //Seed 圖片上傳
        public void UploadFromSeed(string seedFolder,string sourceFile,
            string fileNameWithoutExt,List<ImageSizeOption> sizes)
        {
            using var image = Image.Load(sourceFile);
            ProcessAndSaveImage(image, seedFolder, fileNameWithoutExt, sizes);
        }

        //圖片上傳
        public string UploadFromFormFile(IFormFile file,string folderName,
            List<ImageSizeOption> sizes,string? entityId = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("檔案不存在");
            // ContentType
            var allowedTypes = new[] { "image/jpeg", "image/png","image/jpg" };

            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("只允許上傳 jpg、jpeg 或 png 圖片");
            //副檔名
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExts = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExts.Contains(ext))
                throw new InvalidOperationException("圖片副檔名不正確");

            //ImageSharp 真正解析（最後防線）
            string fileName = entityId ?? Guid.NewGuid().ToString();
            using var stream = file.OpenReadStream();
            Image image;
            try
            {
                image = Image.Load(stream);
            }
            catch
            {
                throw new InvalidOperationException("圖片格式無法解析");
            }

            ProcessAndSaveImage(image, folderName, fileName, sizes);

            return fileName; // 存 DB 用
        }

        //重設尺寸，儲存檔案
        private void ProcessAndSaveImage(
        Image image,
        string folderName,
        string fileNameWithoutExt,
        List<ImageSizeOption> sizes
        )
        {
            string basePhotoPath = Path.Combine(
                _env.WebRootPath,"Photos",folderName);

            foreach (var size in sizes)
            {
                string folderPath = Path.Combine(basePhotoPath, size.FolderName);
                Directory.CreateDirectory(folderPath);

                var options = new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(size.Width, size.Height),
                    Mode = ResizeMode.Crop,
                    Position = AnchorPositionMode.Center
                };

                using var resized = image.Clone(ctx => ctx.Resize(options));

                string destFile = Path.Combine(folderPath,$"{fileNameWithoutExt}.png");

                resized.Save(destFile, new PngEncoder());
            }
        }
    }
}