public enum AutoReplyTemplateTriggerType : byte
{
    OnMessage = 1,      // 收到任何訊息
    FirstMessage = 2,   // 該 Thread 第一則訊息
    Keyword = 3,        // 關鍵字觸發
    AfterOrder = 4      // 訂單成立後
}