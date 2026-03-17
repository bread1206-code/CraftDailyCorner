namespace CraftDailyCorner.Services
{
    public static class ImageSizePresets
    {
        public static List<ImageSizeOption> Member =>
            new()
            {
            new ImageSizeOption { FolderName = "Thumbnail", Width = 100, Height = 100 },
            new ImageSizeOption { FolderName = "Medium", Width = 300, Height = 300 }
            };
        public static List<ImageSizeOption> CreatorApplication =>
            new()
            {
            new ImageSizeOption { FolderName = "Thumbnail", Width = 100, Height = 56 },
            new ImageSizeOption { FolderName = "Large", Width = 800, Height = 450 }
            };
        public static List<ImageSizeOption> Creator =>
            new()
            {
            new ImageSizeOption { FolderName = "Medium", Width = 300, Height = 300 },
            new ImageSizeOption { FolderName = "Large", Width = 800, Height = 450 }
            };

        public static List<ImageSizeOption> Product =>
            new()
            {
            new ImageSizeOption { FolderName = "Medium", Width = 300, Height = 300 },
            new ImageSizeOption { FolderName = "Large", Width = 800, Height = 800 }
            };
        public static List<ImageSizeOption> Post =>
            new()
            {
            new ImageSizeOption { FolderName = "Medium", Width = 300, Height = 300 },
            new ImageSizeOption { FolderName = "Large", Width = 800, Height = 450 }
            };
        public static List<ImageSizeOption> Portfolio =>
            new ()
            {
            new ImageSizeOption { FolderName = "Medium", Width = 300, Height = 300 },
            new ImageSizeOption { FolderName = "Large", Width = 800, Height = 450 }
            };
        public static List<ImageSizeOption> Logo =>
            new()
            {
            new ImageSizeOption { FolderName = "Other", Width = 200, Height = 40 }
            };
        public static List<ImageSizeOption> HomepageBanner =>
            new()
            {
            new ImageSizeOption { FolderName = "Other", Width = 800, Height = 250 }
            };
    }

}
