using CraftDailyCorner.Services;

public interface IImageUploadService
{
    void UploadSeedImage(
        string seedFolder,
        string sourceFile,
        string fileNameWithoutExt,
        List<ImageSizeOption> sizes
    );
}
