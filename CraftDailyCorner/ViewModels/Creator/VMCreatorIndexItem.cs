namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorIndexItem
    {
        public string CreatorID { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Intro { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int FollowerCount { get; set; }

        public string ImagePath =>
            $"/Photos/03CreatorBrand/Medium/{ImageUrl}.png";
    }
}
