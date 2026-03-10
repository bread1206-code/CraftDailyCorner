using CraftDailyCorner.ViewModels.Message;

namespace CraftDailyCorner.Services.Interface
{
    public interface IMessageService
    {
        // 訊息主頁（Inbox + Chat）
        Task<VMMessageIndex> GetInboxAsync(
            string memberId,
            string? creatorId,
            int? threadId);

        // 從商品頁建立 / 取得對話
        Task<int> GetOrCreateThreadFromProductAsync(string memberId, string productId);

        // 發送訊息
        Task SendMessageAsync(int threadId, string senderId, string content);

        // 快速回覆模板
        Task<List<VMQuickReplyTemplateItem>> GetQuickReplyTemplatesAsync(string creatorId);

        // Navbar / 其他頁面判斷是否有未讀訊息
        Task<bool> HasUnreadMessagesAsync(string memberId, string? creatorId);
    }
}