using System;

namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageItem
    {
        public long MessageID { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        // 是否為目前登入者送出的訊息
        public bool IsMine { get; set; }

        // 是否為自動回覆
        public bool IsAutoReply { get; set; }

        // 顯示名稱
        public string SenderName { get; set; } = null!;
    }
}