namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMProductImageItem
    {
        public long ImageID { get; set; }

        public string CreatorID { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public byte SortOrder { get; set; }

        public string ImagePath =>
            $"/Photos/04ProductImage/{CreatorID}/Medium/{ImageUrl}.png";
    }
}