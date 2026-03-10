using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Notification
{
    public class VMNotificationPreference
    {
        [Display(Name = "商品通知")]
        public bool ProductNotificationEnabled { get; set; }

        [Display(Name = "訂單通知")]
        public bool OrderNotificationEnabled { get; set; }

        [Display(Name = "創作者通知")]
        public bool CreatorNotificationEnabled { get; set; }
    }
}