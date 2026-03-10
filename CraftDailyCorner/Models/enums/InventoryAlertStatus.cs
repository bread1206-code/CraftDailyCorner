using System.ComponentModel.DataAnnotations;
namespace CraftDailyCorner.Models.enums
{
    public enum InventoryAlertStatus : byte
    {
        //Pending = 0,	//尚未觸發
        [Display(Name = "已提醒")]
        Triggered = 0,  //已提醒
        [Display(Name = "已處理")]
        Resolved = 1    //已處理
    }
}