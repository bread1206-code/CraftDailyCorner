using System.ComponentModel.DataAnnotations;

public enum NotificationType : byte
{
    [Display(Name = "訊息提醒")]
    Message = 0,	//訊息提醒
    [Display(Name = "出貨通知")]
    Order = 1,	//出貨通知
    [Display(Name = "價格變動通知")]
    PriceChang = 2,	//價格變動通知
    [Display(Name = "創作日誌更新通知")]
    PostUpdata = 3	//創作日誌更新通知
}