using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Homepage
{
    //熱門貼文卡片資料
    public class VMHotPostCard
    {
        public string PostID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yy/MM/dd}")]
        public DateTime CreatedAt { get; set; }

        public string CreatorID { get; set; } = null!;
        public string BrandName { get; set; } = null!;

        public string ImagePath =>
            string.IsNullOrWhiteSpace(ImageUrl)
                ? "/images/no-image.webp"
                : $"/Photos/05CreatorPost/{CreatorID}/Medium/{ImageUrl}.webp";
    }
}
