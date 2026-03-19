using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models.enums
{
    public enum NotificationType : byte
    {
        // ===== 全體 =====
        [Display(Name = "公告通知")]
        Announcement = 1,

        // ===== 會員：收藏商品 =====
        [Display(Name = "收藏商品已上架通知")]
        FavoriteProductPublished = 10,

        [Display(Name = "收藏商品已補貨通知")]
        FavoriteProductRestocked = 11,

        // ===== 會員：追蹤創作者 =====
        [Display(Name = "創作者新日誌通知")]
        CreatorNewPost = 20,

        [Display(Name = "創作者新商品通知")]
        CreatorNewProduct = 21,

        [Display(Name = "創作者新作品集通知")]
        CreatorNewPortfolio = 22,

        // ===== 訂單：會員 =====
        [Display(Name = "訂單已成立通知")]
        OrderCreated = 30,

        [Display(Name = "付款完成通知")]
        OrderPaid = 31,

        [Display(Name = "商品已寄出通知")]
        OrderShipped = 32,

        [Display(Name = "商品已送達通知")]
        OrderDelivered = 33,

        // ===== 商品：創作者 =====
        [Display(Name = "商品低庫存通知")]
        ProductLowStock = 40,

        [Display(Name = "商品缺貨通知")]
        ProductOutOfStock = 41,

        // ===== 創作者互動 / 訂單 =====
        [Display(Name = "日誌回應通知")]
        PostComment = 50,
        // ===== 訂單：創作者 =====
        [Display(Name = "訂單完成通知")]
        OrderCompleted = 51
    }
}