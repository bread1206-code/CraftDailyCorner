using CraftDailyCorner.ViewModels.Message;

namespace CraftDailyCorner.Services.Interface
{
    public interface IMessageService
    {
        /// <summary>
        /// 載入訊息首頁（Inbox + Chat）
        /// </summary>
        /// <param name="memberId">目前登入會員ID</param>
        /// <param name="creatorId">目前登入創作者ID（若不是創作者可為 null）</param>
        /// <param name="threadId">目前選中的對話 ThreadID</param>
        Task<VMMessageIndex> GetInboxAsync(string memberId, string? creatorId, int? threadId);

        /// <summary>
        /// 從商品頁發起詢問：取得既有 thread，若不存在則建立新 thread
        /// </summary>
        /// <param name="memberId">會員ID</param>
        /// <param name="productId">商品ID</param>
        Task<int> GetOrCreateThreadFromProductAsync(string memberId, string productId);

        /// <summary>
        /// 送出訊息
        /// </summary>
        /// <param name="threadId">對話 ThreadID</param>
        /// <param name="senderId">發送者 MemberID</param>
        /// <param name="content">訊息內容</param>
        Task SendMessageAsync(int threadId, string senderId, string content);

        /// <summary>
        /// 將目前使用者在該 thread 中收到的訊息標記為已讀
        /// </summary>
        /// <param name="threadId">對話 ThreadID</param>
        /// <param name="currentMemberId">目前登入會員ID</param>
        Task MarkThreadAsReadAsync(int threadId, string currentMemberId);

        /// <summary>
        /// 取得創作者可用的快速回覆模板
        /// </summary>
        /// <param name="creatorId">創作者ID</param>
        Task<List<VMQuickReplyTemplateItem>> GetQuickReplyTemplatesAsync(string creatorId);
    }
}