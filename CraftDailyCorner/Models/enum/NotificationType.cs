using System.ComponentModel.DataAnnotations;

public enum NotificationType : byte
{
    [Display(Name = "訂單通知")]
    Order = 1,
    [Display(Name = "商品通知")]
    Product = 2,
    [Display(Name = "創作日誌更新通知")]
    CreatorPost = 3,
    [Display(Name = "公告通知")]
    Announcement = 4,
    [Display(Name = "訊息提醒")]
    Message = 5
}