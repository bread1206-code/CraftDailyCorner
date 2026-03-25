namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorIndexItem
    {
        public string CreatorID { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string BrandIntro { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int FollowerCount { get; set; }

        public string ImagePath =>
            $"/Photos/03CreatorBrand/{CreatorID}/Medium/{ImageUrl}.webp";
    }
}
