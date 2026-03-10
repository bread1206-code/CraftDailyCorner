using CraftDailyCorner.Models;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Message;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MessageService : IMessageService
    {
        private readonly CraftDailyCornerContext _context;

        public MessageService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMMessageIndex> GetInboxAsync(
            string memberId,
            string? creatorId,
            int? threadId)
        {
            bool isCreatorSide = !string.IsNullOrWhiteSpace(creatorId);

            // 先把目前進入的對話標記為已讀
            if (threadId.HasValue)
            {
                await MarkThreadAsReadAsync(threadId.Value, memberId, creatorId);
            }

            IQueryable<MessageThread> baseQuery = _context.MessageThreads
                .AsNoTracking()
                .Include(t => t.Member)
                .Include(t => t.CreatorProfile)
                .Include(t => t.Product)
                .Include(t => t.Messages);

            // 創作者端：看自己參與的所有對話
            if (isCreatorSide)
            {
                baseQuery = baseQuery.Where(t =>
                    t.MemberID == memberId ||
                    t.CreatorID == creatorId);
            }
            // 會員端：看自己所有對話
            else
            {
                baseQuery = baseQuery.Where(t => t.MemberID == memberId);
            }

            var threads = await baseQuery
                .OrderByDescending(t => t.LastMessageAt)
                .ToListAsync();

            var conversations = threads.Select(t =>
            {
                bool amCreatorOfThisThread =
                    !string.IsNullOrWhiteSpace(creatorId) && t.CreatorID == creatorId;

                string displayName = amCreatorOfThisThread
                    ? t.Member.DisplayName
                    : t.CreatorProfile.BrandName;

                string? subTitle = amCreatorOfThisThread
                    ? t.Member.MemberID
                    : t.CreatorProfile.CreatorID;

                string mySenderId = amCreatorOfThisThread
                    ? t.CreatorProfile.MemberID
                    : memberId;

                int unreadCount = t.Messages.Count(m => !m.IsRead && m.SenderID != mySenderId);

                return new VMMessageConversationItem
                {
                    ThreadID = t.ThreadID,
                    DisplayName = displayName,
                    SubTitle = subTitle,
                    LastMessagePreview = t.LastMessagePreview,
                    LastMessageAt = t.LastMessageAt,
                    UnreadCount = unreadCount,
                    ProductID = t.ProductID,
                    ProductName = t.Product?.ProductName,
                    IsActive = threadId.HasValue && t.ThreadID == threadId.Value
                };
            }).ToList();

            VMMessageDetail? currentThread = null;

            if (threadId.HasValue)
            {
                currentThread = await BuildMessageDetailAsync(
                    threadId.Value,
                    memberId,
                    creatorId,
                    isCreatorSide);
            }
            else if (conversations.Any())
            {
                var firstThreadId = conversations.First().ThreadID;

                conversations[0].IsActive = true;

                // 自動選第一筆時，也要先標已讀
                await MarkThreadAsReadAsync(firstThreadId, memberId, creatorId);

                // 重新計算 conversations，避免第一筆未讀數還留著
                conversations[0].UnreadCount = 0;

                currentThread = await BuildMessageDetailAsync(
                    firstThreadId,
                    memberId,
                    creatorId,
                    isCreatorSide);

                threadId = firstThreadId;
            }

            return new VMMessageIndex
            {
                Conversations = conversations,
                CurrentThread = currentThread,
                CurrentThreadID = threadId,
                IsCreatorSide = isCreatorSide
            };
        }

        public async Task<int> GetOrCreateThreadFromProductAsync(string memberId, string productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
                throw new ArgumentException("找不到商品資料");

            var productOwnerMemberId = await _context.CreatorProfiles
                .Where(c => c.CreatorID == product.CreatorID)
                .Select(c => c.MemberID)
                .FirstOrDefaultAsync();

            if (productOwnerMemberId == memberId)
                throw new ArgumentException("不能向自己的商品提問");

            var existingThread = await _context.MessageThreads
                .FirstOrDefaultAsync(t =>
                    t.MemberID == memberId &&
                    t.CreatorID == product.CreatorID &&
                    t.ProductID == productId);

            if (existingThread != null)
                return existingThread.ThreadID;

            var now = DateTime.Now;

            var thread = new MessageThread
            {
                MemberID = memberId,
                CreatorID = product.CreatorID,
                ProductID = productId,
                CreatedAt = now,
                LastMessageAt = now,
                LastMessagePreview = "已建立商品詢問對話"
            };

            _context.MessageThreads.Add(thread);
            await _context.SaveChangesAsync();

            return thread.ThreadID;
        }

        public async Task SendMessageAsync(int threadId, string senderId, string content)
        {
            content = (content ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("訊息內容不可為空白");

            var thread = await _context.MessageThreads
                .Include(t => t.CreatorProfile)
                .FirstOrDefaultAsync(t => t.ThreadID == threadId);

            if (thread == null)
                throw new ArgumentException("找不到對話資料");

            var creatorMemberId = thread.CreatorProfile.MemberID;

            if (senderId != thread.MemberID && senderId != creatorMemberId)
                throw new ArgumentException("無權限在此對話中發送訊息");

            var now = DateTime.Now;

            var isFirstUserMessage = !await _context.Messages
                .AnyAsync(m => m.ThreadID == threadId);

            var message = new Message
            {
                ThreadID = threadId,
                SenderID = senderId,
                Content = content,
                CreatedAt = now,
                IsRead = false
            };

            _context.Messages.Add(message);

            thread.LastMessageAt = now;
            thread.LastMessagePreview = BuildPreview(content);

            await _context.SaveChangesAsync();

            // 第一次訊息時，若創作者有啟用 FirstMessage 自動回覆，就自動插入一則
            if (isFirstUserMessage && senderId == thread.MemberID)
            {
                await TrySendFirstMessageAutoReplyAsync(thread);
            }
        }

        public async Task<List<VMQuickReplyTemplateItem>> GetQuickReplyTemplatesAsync(string creatorId)
        {
            return await _context.AutoReplyTemplates
                .AsNoTracking()
                .Where(t =>
                    t.CreatorID == creatorId &&
                    t.IsActive &&
                    t.TriggerType == AutoReplyTemplateTriggerType.QuickReply)
                .OrderBy(t => t.Title)
                .Select(t => new VMQuickReplyTemplateItem
                {
                    TemplateID = t.TemplateID,
                    Title = t.Title,
                    Content = t.Content
                })
                .ToListAsync();
        }

        public async Task<bool> HasUnreadMessagesAsync(string memberId, string? creatorId)
        {
            // =============================
            // 會員身分未讀：
            // 自己作為 Member 參與的對話中，
            // 是否存在「不是自己送的」未讀訊息
            // =============================
            var hasMemberUnread = await _context.Messages
                .AsNoTracking()
                .AnyAsync(m =>
                    !m.IsRead &&
                    m.SenderID != memberId &&
                    m.MessageThread.MemberID == memberId);

            if (hasMemberUnread)
                return true;

            // =============================
            // 創作者身分未讀：
            // 自己作為 Creator 參與的對話中，
            // 是否存在「不是自己（創作者本人）送的」未讀訊息
            // =============================
            if (!string.IsNullOrWhiteSpace(creatorId))
            {
                var creatorMemberId = await _context.CreatorProfiles
                    .AsNoTracking()
                    .Where(c => c.CreatorID == creatorId)
                    .Select(c => c.MemberID)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrWhiteSpace(creatorMemberId))
                {
                    var hasCreatorUnread = await _context.Messages
                        .AsNoTracking()
                        .AnyAsync(m =>
                            !m.IsRead &&
                            m.SenderID != creatorMemberId &&
                            m.MessageThread.CreatorID == creatorId);

                    if (hasCreatorUnread)
                        return true;
                }
            }

            return false;
        }

        // =========================
        // Private Helpers
        // =========================

        private async Task<VMMessageDetail?> BuildMessageDetailAsync(
            int threadId,
            string memberId,
            string? creatorId,
            bool isCreatorSide)
        {
            var thread = await _context.MessageThreads
                .AsNoTracking()
                .Include(t => t.Member)
                .Include(t => t.CreatorProfile)
                .Include(t => t.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(t => t.Messages)
                    .ThenInclude(m => m.Member)
                .FirstOrDefaultAsync(t => t.ThreadID == threadId);

            if (thread == null)
                return null;

            // 權限檢查：只能查看自己參與的對話
            if (thread.MemberID != memberId &&
                (string.IsNullOrWhiteSpace(creatorId) || thread.CreatorID != creatorId))
            {
                return null;
            }

            bool amCreatorOfThisThread =
                !string.IsNullOrWhiteSpace(creatorId) && thread.CreatorID == creatorId;

            string displayName = amCreatorOfThisThread
                ? thread.Member.DisplayName
                : thread.CreatorProfile.BrandName;

            string? subTitle = amCreatorOfThisThread
                ? thread.Member.MemberID
                : thread.CreatorProfile.CreatorID;

            var messages = thread.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new VMMessageItem
                {
                    MessageID = m.MessageID,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    IsRead = m.IsRead,
                    IsMine = amCreatorOfThisThread
                        ? m.SenderID == thread.CreatorProfile.MemberID
                        : m.SenderID == memberId,
                    IsAutoReply = false, // 之後若你要區分模板/自動訊息，可再補欄位
                    SenderName = m.Member.DisplayName
                })
                .ToList();

            var quickReplyTemplates = amCreatorOfThisThread
                ? await GetQuickReplyTemplatesAsync(thread.CreatorID)
                : new List<VMQuickReplyTemplateItem>();

            return new VMMessageDetail
            {
                ThreadID = thread.ThreadID,
                DisplayName = displayName,
                SubTitle = subTitle,
                ProductID = thread.ProductID,
                ProductName = thread.Product?.ProductName,
                ProductPrice = thread.Product?.Price,
                ProductImageUrl = thread.Product?.ProductImages
                    .Where(pi => pi.StatusID == 1)
                    .OrderBy(pi => pi.SortOrder)
                    .Select(pi => pi.ImageUrl)
                    .FirstOrDefault(),
                Messages = messages,
                QuickReplyTemplates = quickReplyTemplates,
                DraftContent = null
            };
        }

        private async Task TrySendFirstMessageAutoReplyAsync(MessageThread thread)
        {
            var template = await _context.AutoReplyTemplates
                .AsNoTracking()
                .Where(t =>
                    t.CreatorID == thread.CreatorID &&
                    t.IsActive &&
                    t.TriggerType == AutoReplyTemplateTriggerType.FirstMessage)
                .OrderBy(t => t.TemplateID)
                .FirstOrDefaultAsync();

            if (template == null)
                return;

            var creatorMemberId = await _context.CreatorProfiles
                .Where(c => c.CreatorID == thread.CreatorID)
                .Select(c => c.MemberID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(creatorMemberId))
                return;

            var now = DateTime.Now;

            _context.Messages.Add(new Message
            {
                ThreadID = thread.ThreadID,
                SenderID = creatorMemberId,
                Content = template.Content,
                CreatedAt = now,
                IsRead = false
            });

            thread.LastMessageAt = now;
            thread.LastMessagePreview = BuildPreview(template.Content);

            await _context.SaveChangesAsync();
        }

        private static string BuildPreview(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            content = content.Trim();

            return content.Length <= 50
                ? content
                : content.Substring(0, 50);
        }

        private async Task MarkThreadAsReadAsync(int threadId, string memberId, string? creatorId)
        {
            var thread = await _context.MessageThreads
                .Include(t => t.CreatorProfile)
                .Include(t => t.Messages)
                .FirstOrDefaultAsync(t => t.ThreadID == threadId);

            if (thread == null)
                return;

            bool amCreatorOfThisThread =
                !string.IsNullOrWhiteSpace(creatorId) && thread.CreatorID == creatorId;

            string mySenderId = amCreatorOfThisThread
                ? thread.CreatorProfile.MemberID
                : memberId;

            var unreadMessages = thread.Messages
                .Where(m => !m.IsRead && m.SenderID != mySenderId)
                .ToList();

            if (!unreadMessages.Any())
                return;

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}