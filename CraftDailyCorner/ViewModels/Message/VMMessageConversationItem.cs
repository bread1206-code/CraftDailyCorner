using System;

namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageConversationItem
    {
        public int ThreadID { get; set; }

        // 對話對象顯示名稱
        public string DisplayName { get; set; } = null!;

        // 對話對象副標題
        public string? SubTitle { get; set; }

        // 最後訊息摘要
        public string? LastMessagePreview { get; set; }

        // 最後訊息時間
        public DateTime LastMessageAt { get; set; }

        // 未讀數
        public int UnreadCount { get; set; }

        // 商品上下文
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }

        // 是否目前選中
        public bool IsActive { get; set; }
    }
}