using CraftDailyCorner.Services.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace CraftDailyCorner.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _env;

        // 你可以依需求微調
        private const int WebpQuality = 75;

        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string UploadImage(
            IFormFile? file,
            string? seedSourcePath,
            string folderName,
            List<ImageSizeOption> sizes,
            string? entityId = null,
            string? entitySubFolder = null)
        {
            // Seed 圖片
            if (file == null && !string.IsNullOrWhiteSpace(seedSourcePath))
            {
                var finalFileName = entityId ?? Guid.NewGuid().ToString();

                UploadFromSeed(
                    seedFolder: folderName,
                    sourceFile: seedSourcePath,
                    fileNameWithoutExt: finalFileName,
                    sizes: sizes,
                    entitySubFolder: entitySubFolder
                );

                return finalFileName;
            }

            // 使用者上傳
            if (file != null)
            {
                return UploadFromFormFile(
                    file,
                    folderName,
                    sizes,
                    entityId,
                    entitySubFolder
                );
            }

            throw new InvalidOperationException("無有效圖片來源");
        }

        // Seed 圖片上傳
        public void UploadFromSeed(
            string seedFolder,
            string sourceFile,
            string fileNameWithoutExt,
            List<ImageSizeOption> sizes,
            string? entitySubFolder = null)
        {
            var resolvedSourcePath = ResolveSourcePath(sourceFile);

            if (string.IsNullOrWhiteSpace(resolvedSourcePath) || !File.Exists(resolvedSourcePath))
                throw new FileNotFoundException("Seed 圖片來源不存在", resolvedSourcePath);

            using var image = Image.Load(resolvedSourcePath);
            image.Mutate(x => x.AutoOrient());

            ProcessAndSaveImage(
                image,
                seedFolder,
                fileNameWithoutExt,
                sizes,
                entitySubFolder
            );
        }

        // 表單圖片上傳
        public string UploadFromFormFile(
            IFormFile file,
            string folderName,
            List<ImageSizeOption> sizes,
            string? entityId = null,
            string? entitySubFolder = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("檔案不存在");

            ValidateUploadedFile(file);

            string fileName = entityId ?? Guid.NewGuid().ToString();

            using var stream = file.OpenReadStream();
            using var image = LoadImageFromStream(stream);

            image.Mutate(x => x.AutoOrient());

            ProcessAndSaveImage(
                image,
                folderName,
                fileName,
                sizes,
                entitySubFolder
            );

            return fileName; // DB 只存檔名主體，不含副檔名
        }

        // 重設尺寸，儲存檔案
        private void ProcessAndSaveImage(
            Image image,
            string folderName,
            string fileNameWithoutExt,
            List<ImageSizeOption> sizes,
            string? entitySubFolder = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("folderName 不可為空", nameof(folderName));

            if (sizes == null || sizes.Count == 0)
                throw new ArgumentException("sizes 不可為空", nameof(sizes));

            string basePhotoPath = string.IsNullOrWhiteSpace(entitySubFolder)
                ? Path.Combine(_env.WebRootPath, "Photos", folderName)
                : Path.Combine(_env.WebRootPath, "Photos", folderName, entitySubFolder);

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

                string destFile = Path.Combine(folderPath, $"{fileNameWithoutExt}.webp");

                var encoder = new WebpEncoder
                {
                    Quality = WebpQuality,
                    FileFormat = WebpFileFormatType.Lossy
                };

                resized.Save(destFile, encoder);
            }
        }

        private static void ValidateUploadedFile(IFormFile file)
        {
            var allowedTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/jpg",
                "image/webp"
            };

            if (!allowedTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new InvalidOperationException("只允許上傳 jpg、jpeg、png 或 webp 圖片");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExts.Contains(ext))
                throw new InvalidOperationException("圖片副檔名不正確");
        }

        private static Image LoadImageFromStream(Stream stream)
        {
            try
            {
                return Image.Load(stream);
            }
            catch
            {
                throw new InvalidOperationException("圖片格式無法解析");
            }
        }

        private string ResolveSourcePath(string sourceFile)
        {
            if (Path.IsPathRooted(sourceFile))
                return sourceFile;

            return Path.Combine(_env.ContentRootPath, sourceFile);
        }
    }
}