namespace CraftDailyCorner.Models.enums
{
    public enum AutoReplyTemplateTriggerType : byte
    {
        FirstMessage = 1,   // 該 Thread 第一則訊息，內容如：您好，感謝您的訊息，我們會盡快回覆您
        QuickReply = 2      // 快速回覆模板（創作者手動使用）
    }
}