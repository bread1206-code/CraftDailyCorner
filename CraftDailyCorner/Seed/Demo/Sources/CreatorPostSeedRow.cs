namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class CreatorPostSeedRow
    {
        // 給 seed 對照用，不直接進 DB
        public string CsvPostID { get; set; } = null!;

        public string BrandName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        // 先當成圖片來源 key / 檔名欄位，之後若要接圖片上傳再擴充
        public string CoverImage { get; set; } = null!;

        // Public / Followers / Private
        public string Visibility { get; set; } = null!;

        // Active / Suspended / Deleted
        public string Status { get; set; } = null!;
    }
}