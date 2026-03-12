using CraftDailyCorner.Services;

public interface IImageUploadService
{
    // 統一入口
    string UploadImage(
        IFormFile? file,
        string? seedSourcePath,
        string folderName,
        List<ImageSizeOption> sizes,
        string? entityId = null,
        string? entitySubFolder = null
    );

    // Seed 專用（給 SeedRunner 用）
    void UploadFromSeed(
        string seedFolder,
        string sourceFile,
        string fileNameWithoutExt,
        List<ImageSizeOption> sizes,
        string? entitySubFolder = null
    );
}