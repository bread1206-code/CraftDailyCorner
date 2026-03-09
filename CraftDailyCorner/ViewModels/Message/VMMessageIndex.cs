using System.Collections.Generic;

namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageIndex
    {
        // 左側對話列表
        public List<VMMessageConversationItem> Conversations { get; set; } = new();

        // 目前選中的對話
        public VMMessageDetail? CurrentThread { get; set; }

        // 目前選中的 ThreadID
        public int? CurrentThreadID { get; set; }

        // 是否為創作者端
        public bool IsCreatorSide { get; set; }
    }
}