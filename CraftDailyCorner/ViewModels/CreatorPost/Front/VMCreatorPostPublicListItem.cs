using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost.Front
{
    public class VMCreatorPostPublicListItem
    {
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        [Display(Name = "創作者")]
        public string CreatorName { get; set; } = null!;

        [Display(Name = "發佈時間")]
        public DateTime CreatedAt { get; set; }

        public string CoverImagePath =>
            $"/Photos/05CreatorPost/Medium/{ImageUrl}.png";

        public string Url =>
            $"/Post/Detail/{PostID}";
    }
}
