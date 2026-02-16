namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMProductImageItem
    {
        public long ImageID { get; set; }

        public string ImageUrl { get; set; } = null!;

        public byte SortOrder { get; set; }



        public string ImagePath =>
            $"/Photos/04ProductImage/Medium/{ImageUrl}.png";
    }
}