using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorDashboard
    {
        // 基本資料

        public string CreatorID { get; set; } = null!;

        [Display(Name = "創作者名稱")]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "頭像")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "創作者簡介")]
        public string Intro { get; set; } = null!;

        [Display(Name = "創作起始日")]
        public DateTime StartDate { get; set; }

        [Display(Name = "加入時間")]
        public DateTime CreatedAt { get; set; }

        //統計資料

        [Display(Name = "商品數量")]
        public int ProductCount { get; set; }

        [Display(Name = "日誌數量")]
        public int PostCount { get; set; }

        [Display(Name = "作品集數量")]
        public int PortfolioCount { get; set; }

        [Display(Name = "粉絲數量")]
        public int FollowerCount { get; set; }

        [Display(Name = "每月銷售額")]
        public decimal MonthlySales { get; set; }

        // UI 輔助屬性

        public string ProfileImagePath =>
            $"/Photos/Creator/Medium/{ImageUrl}.png";

        public int YearsOfExperience =>
            DateTime.Today.Year - StartDate.Year;
    }
}