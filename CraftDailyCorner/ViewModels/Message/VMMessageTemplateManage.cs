namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageTemplateManage
    {
        public List<VMMessageTemplateManageItem> Items { get; set; } = new();

        // 快速回覆模板數量
        public int QuickReplyCount { get; set; }

        // 快速回覆模板上限
        public int QuickReplyLimit { get; set; } = 10;
    }
}