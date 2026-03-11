namespace CraftDailyCorner.Areas.Admin.ViewModels.PlatformSetting
{
    public class VMAdminPlatformSettingIndex
    {
        public List<VMAdminPlatformSettingIndexItem> Items { get; set; } = new();
    }

    public class VMAdminPlatformSettingIndexItem
    {
        public int SettingID { get; set; }

        public string SettingKey { get; set; } = null!;

        public string SettingValue { get; set; } = null!;

        public string DataType { get; set; } = null!;

        public byte CategoryID { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; } = null!;

        public string? UpdatedByName { get; set; }
    }
}