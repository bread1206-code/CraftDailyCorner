using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMHotProductCard
    {
        [Display(Name ="商品編號")]
        public string ProductID { get; set; } = null!;
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;
        [Display(Name = "價格")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }
        [Display(Name = "收藏數")]
        public int FavoriteCount { get; set; }
        [Display(Name = "圖片")]
        public string? CoverImage { get; set; }
        [Display(Name = "創作者")]
        public string CreatorName { get; set; } = null!;
    }
}
